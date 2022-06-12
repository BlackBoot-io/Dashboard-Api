namespace BlackBoot.Services.Implementations;

public class WalletPoolService : IWalletPoolService
{
    private readonly BlackBootDBContext _context;
    private readonly DbSet<WalletPool> _walletPool;

    public WalletPoolService(BlackBootDBContext context)
    {
        _walletPool = context.Set<WalletPool>();
        _context = context;
    }

    public async Task<IActionResponse<WalletPool>> MapUserAsync(Guid userId, BlockchainNetwork network)
    {
        WalletPool wallet = await FindByUserIdAsync(userId, network);
        if (wallet is null)
            return await GetByNetworkAsync(userId, network);
        else
            return new ActionResponse<WalletPool>(wallet);
    }

    private async Task<WalletPool> FindByUserIdAsync(Guid userId, BlockchainNetwork network)
        => await _walletPool.FirstOrDefaultAsync(X => X.UserId == userId && X.Network == network);

    private async Task<IActionResponse<WalletPool>> GetByNetworkAsync(Guid userId, BlockchainNetwork network)
    {
        var wallet = await _walletPool.FirstOrDefaultAsync(X => !X.IsUsed && X.Network == network);
        if (wallet is null)
            return new ActionResponse<WalletPool>(ActionResponseStatusCode.ServerError, AppResource.NoWalletExist);

        wallet.UserId = userId;
        wallet.IsUsed = true;

        var dbResult = _context.SaveChanges();
        if (!dbResult.ToSaveChangeResult())
            return new ActionResponse<WalletPool>(ActionResponseStatusCode.ServerError);

        return new ActionResponse<WalletPool>(wallet);
    }
}