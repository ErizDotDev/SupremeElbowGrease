using QLess.Core.Data;
using QLess.Core.Domain;
using QLess.Infrastructure.Processors;
using Xunit;

namespace QLess.Infrastructure.UnitTests.Processors.Card.RegularCardTransactionProcessorTests
{
	public class GetTripFareShould
	{
		private readonly BaseCardTransactionProcessor cardTransactionProcessor;

		public GetTripFareShould()
		{
			cardTransactionProcessor = new RegularCardTransactionProcessor();
		}

		[Fact]
		public void Return15_WithZeroTransactions()
		{
			var result = cardTransactionProcessor.GetTripFare(new List<Transaction>());

			Assert.Equal(15m, result);
		}

		[Fact]
		public void Run15_WithOneTransaction()
		{
			var transactionsList = new List<Transaction>()
			{
				new Transaction { CardId = 1, Id = 2, TransactionDate = DateTime.Now, TransactionTypeId = TransactionType.PayTrip.Id, TransactionAmount = 8m, PreviousBalance = 500m, NewBalance = 492m }
			};

			var result = cardTransactionProcessor.GetTripFare(transactionsList);

			Assert.Equal(15m, result);
		}
	}
}
