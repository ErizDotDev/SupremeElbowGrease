using QLess.Core.Domain;
using QLess.Web.Models;

namespace QLess.Web.Interfaces
{
	public interface ICardClientService
	{
		Task<CreateCardResponse> CreateCard(CreateCardRequest createCardRequest);
		Task<TripPaymentResponse> PayTrip(string cardNumber);
	}
}
