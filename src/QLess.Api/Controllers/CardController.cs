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

		public CardController(ICardService cardService)
		{
			_cardService = cardService;
		}

		[HttpPost]
		[Route("api/card/create")]
		public async Task<IActionResult> CreateCard([FromBody]CreateCardRequest request)
		{
			return Ok(await _cardService.CreateCard((CardType)request.CardType, request.InitialLoadAmount));
		}
	}
}
