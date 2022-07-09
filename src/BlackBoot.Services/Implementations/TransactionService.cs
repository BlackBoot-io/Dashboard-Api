namespace BlackBoot.Services.Implementations;

public class TransactionService : ITransactionService
{
    private readonly BlackBootDBContext _context;
    private readonly DbSet<Transaction> _transactions;
    private readonly IWalletPoolService _walletPoolService;
    private readonly ICrowdSaleScheduleService _crowdSaleScheduleService;
    private readonly IUserService _userService;

    public TransactionService(BlackBootDBContext context,
        IWalletPoolService walletPoolService,
        ICrowdSaleScheduleService crowdSaleScheduleService,
        IUserService userService)
    {
        _context = context;
        _transactions = context.Set<Transaction>();
        _walletPoolService = walletPoolService;
        _crowdSaleScheduleService = crowdSaleScheduleService;
        _userService = userService;
    }
    public async Task<IActionResponse<TransactionDto>> Add(Transaction trx)
    {
        if (trx.TokenCount == 0)
            return new ActionResponse<TransactionDto>
            {
                IsSuccess = false,
                Message = AppResource.TokenCountMustBeMoreThanZero
            };

        if (trx.Type == TransactionType.Deposit)
        {
            var crowdSale = await _crowdSaleScheduleService.GetCurrentSaleAsync();
            if (crowdSale.Data is null || !crowdSale.Data.InvestmentIsAvailable())
                return new ActionResponse<TransactionDto>
                {
                    IsSuccess = false,
                    Message = AppResource.CrowdSaleEnded
                };

            if (crowdSale.Data.MinimumBuy > trx.UsdtAmount)
                return new ActionResponse<TransactionDto>
                {
                    IsSuccess = false,
                    Message = string.Format(AppResource.MinimumPayment, crowdSale.Data.MinimumBuy)
                };

            var wallet = await _walletPoolService.MapUserAsync(trx.UserId, trx.Network);
            if (!wallet.IsSuccess)
                return new ActionResponse<TransactionDto>
                {
                    IsSuccess = false,
                    Message = wallet.Message
                };

            trx.TransactionId = Guid.NewGuid();
            trx.BonusCount = crowdSale.Data.BonusCount;
             
            trx.CrowdSaleScheduleId = crowdSale.Data.CrowdSaleScheduleId;
            trx.WalletAddress = wallet.Data.Address;
        }
        else
        {

            var currentUser = await _userService.GetAsync(trx.UserId);
            if (!currentUser.IsSuccess)
                return new ActionResponse<TransactionDto>
                {
                    IsSuccess = false,
                    Message = AppResource.UserNotFound
                };
            if (string.IsNullOrEmpty(currentUser.Data.WithdrawalWallet))
                return new ActionResponse<TransactionDto>
                {
                    IsSuccess = false,
                    Message = AppResource.WithdrawalWalletNotFound
                };

            trx.WalletAddress = currentUser.Data.WithdrawalWallet;
            var balance = await GetUserBalance(trx.UserId);
            if (balance.Data < trx.TokenCount)
                return new ActionResponse<TransactionDto>
                {
                    IsSuccess = false,
                    Message = AppResource.InsufficientBalance
                };
        }

        trx.TransactionId = Guid.NewGuid();
        trx.Date = DateTime.Now;
        trx.Status = TransactionStatus.Pending;
        trx.Type = trx.Type;
        _transactions.Add(trx);
        var dbResult = await _context.SaveChangesAsync();
        if (!dbResult.ToSaveChangeResult())
            return new ActionResponse<TransactionDto>(ActionResponseStatusCode.ServerError, AppResource.TransactionFailed);

        return new ActionResponse<TransactionDto>(new TransactionDto
        {
            TransactionId = trx.TransactionId,
            Network = trx.Network,
            WalletAddress = trx.WalletAddress
        });
    }

    public async Task<IActionResponse<IEnumerable<Transaction>>> GetAll(Guid userid)
        => new ActionResponse<IEnumerable<Transaction>>(await _transactions.Where(X => X.UserId == userid)
            .OrderByDescending(X=>X.TransactionId)
            .AsNoTracking()
            .ToListAsync());

    public async Task<IActionResponse<Transaction>> GetById(Guid transactionId)
        => new ActionResponse<Transaction>(await _transactions.Include(X => X.CrowdSaleSchedule).FirstOrDefaultAsync(X => X.TransactionId == transactionId));

    public async Task<IActionResponse<int>> GetUserBalance(Guid userid)
    {
        var trxs = await _transactions.Where(X => X.UserId == userid &&
                                             X.Status != TransactionStatus.RejectByNetwork &&
                                             X.Status != TransactionStatus.RejectByUser)
                                       .AsNoTracking()
                                       .ToListAsync();

        var deposits = trxs.Where(X => X.Type == TransactionType.Deposit &&
                                       X.Status == TransactionStatus.ConfirmedByNetwork)
                           .Sum(X => X.TotalToken);

        var withdraw = trxs.Where(X => X.Type == TransactionType.Withdraw).Sum(X => X.TotalToken);

        return new ActionResponse<int>(deposits - withdraw);
    }

    public async Task<IActionResponse<int>> Update(Transaction model)
    {
        var trx = await _transactions.FirstOrDefaultAsync(X => X.TransactionId == model.TransactionId);
        if (trx is null)
            return new ActionResponse<int>(ActionResponseStatusCode.BadRequest);

        trx.TokenCount = model.TokenCount;
        trx.ConfirmDate = model.ConfirmDate;
        trx.CryptoAmount = model.CryptoAmount;
        trx.UsdtAmount = model.UsdtAmount;
        trx.WalletAddress = model.WalletAddress;
        trx.Status = model.Status;
        trx.TxId = model.TxId;

        var dbResult = await _context.SaveChangesAsync();
        if (!dbResult.ToSaveChangeResult())
            return new ActionResponse<int>(ActionResponseStatusCode.ServerError, AppResource.TransactionFailed);

        return new ActionResponse<int>();
    }
}

