#nullable disable
namespace BlackBoot.Services.Interfaces;

public interface ITransactionService : IScopedDependency
{
    Task<IActionResponse<TransactionDto>> Add(Transaction trx);
    Task<IActionResponse<int>> Update(Transaction model);
    Task<IActionResponse<IEnumerable<Transaction>>> GetAll(Guid userid);
    Task<IActionResponse<Transaction>> GetById(Guid transactionId);
    Task<IActionResponse<int>> GetUserBalance(Guid userid);
}