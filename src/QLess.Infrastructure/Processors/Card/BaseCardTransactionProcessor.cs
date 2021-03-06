using QLess.Core.Domain;
using QLess.Core.Enums;

namespace QLess.Infrastructure.Processors
{
	public abstract class BaseCardTransactionProcessor
	{
		internal const decimal MaximumLoadTransactionAmount = 1000m;		

		public static Dictionary<CardType, Func<BaseCardTransactionProcessor>> GetAvailableTransactionProcessors()
			=> new Dictionary<CardType, Func<BaseCardTransactionProcessor>>()
			{
				{ CardType.Regular, () => new RegularCardTransactionProcessor() },
				{ CardType.Discounted, () => new DiscountedCardTransactionProcessor() }
			};

		public virtual CreateCardResponse TryCreateCardNumber(decimal initialLoadAmount, string specialIdNumber = "")
			=> new CreateCardResponse();

		public virtual decimal GetTripFare(List<Transaction> currentDateTripTransactions) => 0m;

		public virtual bool IsCardExpired(DateTime dateLastUsed, DateTime dateUsed) => false;

		internal string GenerateCardNumber()
			=> DateTime.Now.ToString("yyddMMhhmmss");
	}
}
