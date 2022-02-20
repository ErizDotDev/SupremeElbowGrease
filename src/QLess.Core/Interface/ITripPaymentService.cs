using QLess.Core.Domain;

namespace QLess.Core.Interface
{
	public interface ITripPaymentService
	{
		Task<TripPaymentResponse> PayForTrip(string cardNumber);
	}
}
