using QLess.Core.Services;

namespace QLess.Core.Interface
{
	public interface ITripPaymentService
	{
		Task<ServiceResponse> PayForTrip(string cardNumber);
	}
}
