namespace BlackBoot.Services.Implementations;

public class TransactionService : ITransactionService
{
    private readonly BlackBootDBContext _context;
    private readonly DbSet<Transaction> _transactions;
    private readonly IWalletPoolService _walletPoolService;
    private readonly ICrowdSaleScheduleService _crowdSaleScheduleService;

    public TransactionService(BlackBootDBContext context,
        IWalletPoolService walletPoolService,
        ICrowdSaleScheduleService crowdSaleScheduleService)
    {
        _context = context;
        _transactions = context.Set<Transaction>();
        _walletPoolService = walletPoolService;
        _crowdSaleScheduleService = crowdSaleScheduleService;
    }
    public async Task<IActionResponse<TransactionDto>> Add(Transaction trx)
    {
        var wallet = await _walletPoolService.MapUserAsync(trx.UserId, trx.Network);
        if (!wallet.IsSuccess)
            return new ActionResponse<TransactionDto>(ActionResponseStatusCode.ServerError);

        var crowdSale = await _crowdSaleScheduleService.GetCurrentSale();
        if (crowdSale is null || !crowdSale.InvestmentIsAvailable())
            return new ActionResponse<TransactionDto>(ActionResponseStatusCode.Success, AppResource.CrowdSaleEnded);

        trx.TransactionId = Guid.NewGuid();
        trx.BonusCount = crowdSale.BonusCount;
        trx.CrowdSaleScheduleId = crowdSale.CrowdSaleScheduleId;
        trx.Date = DateTime.Now;
        trx.Status = TransactionStatus.Pending;
        trx.Type = TransactionType.Deposit;

        var dbResult = await _context.SaveChangesAsync();
        if (!dbResult.ToSaveChangeResult())
            return new ActionResponse<TransactionDto>(ActionResponseStatusCode.ServerError, AppResource.TransactionFailed);

        return new ActionResponse<TransactionDto>(new TransactionDto
        {
            TransactionId = trx.TransactionId,
            Network = trx.Network,
            WalletAddress = wallet.Data.Address,
        });
    }

    public async Task<IActionResponse<IEnumerable<Transaction>>> GetAll(Guid userid)
        => new ActionResponse<IEnumerable<Transaction>>(await _transactions.Where(X => X.UserId == userid).AsNoTracking().ToListAsync());

    public async Task<IActionResponse<int>> GetUserBalance(Guid userid)
    {
        var trxs = await _transactions.Where(X => X.UserId == userid &&
                                             X.Status == TransactionStatus.ConfirmedByNetwork)
                                       .AsNoTracking()
                                       .ToListAsync();

        var deposits = trxs.Where(X => X.Type == TransactionType.Deposit).Sum(X => X.TotalToken);
        var withdraw = trxs.Where(X => X.Type == TransactionType.Withdraw).Sum(X => X.TotalToken);

        return new ActionResponse<int>(deposits - withdraw);
    }

    public async Task<IActionResponse<Transaction>> Update(Transaction model)
    {
        var trx = await GetTransactionById(model.TransactionId);
        if (trx is null)
            return new ActionResponse<Transaction>(ActionResponseStatusCode.BadRequest);

        trx.TokenCount = model.TokenCount;
        trx.ConfirmDate = model.ConfirmDate;
        trx.CryptoAmount = model.CryptoAmount;
        trx.UsdtAmount = model.UsdtAmount;
        trx.WalletAddress = model.WalletAddress;

        var dbResult = await _context.SaveChangesAsync();
        if (!dbResult.ToSaveChangeResult())
            return new ActionResponse<Transaction>(ActionResponseStatusCode.ServerError, AppResource.TransactionFailed);

        return new ActionResponse<Transaction>(trx);
    }

    private async Task<Transaction> GetTransactionById(Guid trxId)
        => await _transactions.FirstOrDefaultAsync(X => X.TransactionId == trxId);
}

