namespace BlackBoot.Services.Interfaces;

public interface IWalletPoolService : IScopedDependency
{
    Task<IActionResponse<WalletPool>> MapUserAsync(Guid userId, BlockchainNetwork network);
}