using QLess.Core.Domain;

namespace QLess.Core.Interface
{
	public interface ITransactionRepository : IRepository<Transaction>
	{
		List<Transaction> GetTripTransactionsForGivenDate(DateTime targetDate);
	}
}
