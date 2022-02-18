using QLess.Core.Domain;

namespace QLess.Core.Interface
{
	public interface ITransactionService
	{
		Task<bool> SaveCreateCardTransaction(Card cardDetail);
		Task<bool> SaveTripPaymentTransaction(Card cardDetail, decimal fare);
	}
}
