using QLess.Core.Domain;

namespace QLess.Infrastructure.Processors
{
	public class RegularCardTransactionProcessor : BaseCardTransactionProcessor
	{
		public override CreateCardResponse TryCreateCardNumber(decimal initialLoadAmount, string specialIdNumber = "")
		{
			const decimal minimumRegularCardTypeLoad = 100;

			if (initialLoadAmount < minimumRegularCardTypeLoad)
			{
				return new CreateCardResponse
				{
					CardNumber = string.Empty,
					ErrorMessage = $"Minimum initial load balance not reached. Please load your card with at least P{minimumRegularCardTypeLoad}.00"
				};
			}

			if (initialLoadAmount > MaximumLoadTransactionAmount)
			{
				return new CreateCardResponse
				{
					CardNumber = string.Empty,
					ErrorMessage = $"Exceeded max load amount per transaction P{MaximumLoadTransactionAmount}.00."
				};
			}

			string cardNumber = GenerateCardNumber();

			return new CreateCardResponse { CardNumber = cardNumber };
		}

		public override decimal GetTripFare(List<Transaction> currentDateTripTransactions) => 15m;
	}
}
