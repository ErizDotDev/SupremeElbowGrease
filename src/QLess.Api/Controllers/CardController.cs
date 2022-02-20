using Microsoft.AspNetCore.Mvc;
using QLess.Api.Models.Request;
using QLess.Core.Enums;
using QLess.Core.Interface;

namespace QLess.Api.Controllers
{
	[ApiController]	
	public class CardController : ControllerBase
	{
		private readonly ICardService _cardService;
		private readonly ICardLoadService _cardLoadService;

		public CardController(ICardService cardService, ICardLoadService cardLoadService)
		{
			_cardService = cardService;
			_cardLoadService = cardLoadService;
		}

		[HttpPost]
		[Route("api/card/create")]
		public async Task<IActionResult> CreateCard([FromBody]CreateCardRequest request)
		{
			var response = await _cardService.CreateCard(
				(CardType)request.CardType, 
				request.InitialLoadAmount, 
				request.SpecialIDNumber);

			if (response == null)
				return BadRequest("Create card transaction failed.");

			return Ok(response);
		}

		[HttpPost]
		[Route("api/card/load")]
		public async Task<IActionResult> LoadCard([FromBody] LoadCardRequest request)
		{
			var response = await _cardLoadService.LoadCard(
				request.CardNumber,
				request.LoadAmount,
				request.AmountPaid
			);

			if (response == null)
				return BadRequest("Load card transaction failed.");

			return Ok(response);
		}
	}
}
