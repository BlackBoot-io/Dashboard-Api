namespace BlackBoot.Api.Controllers;

public class TransactionController : BaseController
{
    private readonly ITransactionService _transactionService;
    public TransactionController(ITransactionService transactionService)
        => _transactionService = transactionService;

    [HttpPost]
    public async Task<IActionResult> Add(Transaction trx)
        => Ok(await _transactionService.Add(trx));

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid userId)
        => Ok(await _transactionService.GetAll(userId));

    [HttpGet]
    public async Task<IActionResult> GetUserBalance(Guid userId)
        => Ok(await _transactionService.GetUserBalance(userId));

    [HttpPost] 
    public async Task<IActionResult> Update(Transaction trx)
        => Ok(await _transactionService.Update(trx));
}