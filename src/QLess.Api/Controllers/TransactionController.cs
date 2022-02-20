using Microsoft.AspNetCore.Mvc;
using QLess.Core.Interface;

namespace QLess.Api.Controllers
{
	[ApiController]
	public class TransactionController : ControllerBase
	{
		private readonly ITransactionService _transactionService;

		public TransactionController(ITransactionService transactionService)
		{
			_transactionService = transactionService;
		}

		[HttpPost]
		[Route("api/transaction/history")]
		public async Task<IActionResult> GetCardTransactionHistory([FromBody] string cardNumber)
		{
			var response = await _transactionService.GetCardTransactions(cardNumber);

			if (response == null)
				return BadRequest("Failed to get transaction history.");

			return Ok(response);
		}
	}
}
