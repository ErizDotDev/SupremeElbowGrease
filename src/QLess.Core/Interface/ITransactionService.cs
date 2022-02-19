using QLess.Core.Domain;

namespace QLess.Core.Interface
{
	public interface ITransactionService
	{
		Task<bool> SaveCreateCardTransaction(Card cardDetail);
		Task<bool> SaveTripPaymentTransaction(Card cardDetail, decimal fare);
		Task<List<Transaction>> GetTripTransactionsFromGivenDate(DateTime givenDate);
		Task<bool> SaveLoadCardTransaction(Card cardDetail, LoadPaymentDetail computedPaymentValues);
	}
}
