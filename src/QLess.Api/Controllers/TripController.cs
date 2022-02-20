using Microsoft.AspNetCore.Mvc;
using QLess.Core.Interface;

namespace QLess.Api.Controllers
{
	[ApiController]
	public class TripController : ControllerBase
	{
		private readonly ITripPaymentService _tripPaymentService;

		public TripController(ITripPaymentService tripPaymentService)
		{
			_tripPaymentService = tripPaymentService;
		}

		[HttpPost]
		[Route("api/trip/pay")]
		public async Task<IActionResult> PayTrip([FromBody] string cardNumber)
		{
			var response = await _tripPaymentService.PayForTrip(cardNumber);

			if (response == null)
				return BadRequest("Trip payment transaction failed.");

			return Ok(response);
		}
	}
}
