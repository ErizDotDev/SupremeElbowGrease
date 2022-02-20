using QLess.Core.Domain;

namespace QLess.Core.Interface
{
	public interface ITransactionRepository : IRepository<Transaction>
	{
		List<Transaction> GetTripTransactionsForGivenDate(long cardId, DateTime targetDate);
		List<Transaction> GetCardTransactions(string cardNumber);
	}
}
