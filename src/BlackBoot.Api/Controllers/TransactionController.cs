namespace BlackBoot.Api.Controllers;

public class TransactionController : BaseController
{
    private readonly ITransactionService _transactionService;
    public TransactionController(ITransactionService transactionService)
        => _transactionService = transactionService;

    [HttpPost]
    public async Task<IActionResult> AddAsync(Guid userId, [FromBody] Transaction trx)
    {
        trx.UserId = userId;
        return Ok(await _transactionService.Add(trx));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(Guid userId)
        => Ok(await _transactionService.GetAll(userId));

    [HttpGet]
    public async Task<IActionResult> GetByIdAsync(Guid transactionId)
      => Ok(await _transactionService.GetById(transactionId));

    [HttpGet]
    public async Task<IActionResult> GetUserBalanceAsync(Guid userId)
        => Ok(await _transactionService.GetUserBalance(userId));

    [HttpPost]
    public async Task<IActionResult> UpdateAsync(Transaction trx)
        => Ok(await _transactionService.Update(trx));
}