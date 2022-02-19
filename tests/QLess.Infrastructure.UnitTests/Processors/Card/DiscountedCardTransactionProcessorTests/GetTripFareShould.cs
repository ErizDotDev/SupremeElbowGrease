using QLess.Core.Data;
using QLess.Core.Domain;
using QLess.Infrastructure.Processors;
using Xunit;

namespace QLess.Infrastructure.UnitTests.Processors.Card.DiscountedCardTransactionProcessorTests
{
	public class GetTripFareShould
	{
		private readonly BaseCardTransactionProcessor cardTransactionProcessor;

		public GetTripFareShould()
		{
			cardTransactionProcessor = new DiscountedCardTransactionProcessor();
		}

		[Fact]
		public void Return8_GivenNullTransactionList()
		{
			var result = cardTransactionProcessor.GetTripFare(null);

			Assert.Equal(8m, result);
		}

		[Fact]
		public void Return8_GivenZeroTransactions()
		{
			var result = cardTransactionProcessor.GetTripFare(new List<Transaction>());

			Assert.Equal(8m, result);
		}

		[Fact]
		public void Return7Point7_GivenOneTripTransaction()
		{
			var transactionsList = new List<Transaction>()
			{ 
				new Transaction { CardId = 1, Id = 2, TransactionDate = DateTime.Now, TransactionTypeId = TransactionType.PayTrip.Id, TransactionAmount = 8m, PreviousBalance = 500m, NewBalance = 492m }
			};

			var result = cardTransactionProcessor.GetTripFare(transactionsList);

			Assert.Equal(7.7m, result);
		}

		[Fact]
		public void Return7Point7_GivenTwoTripTransactions()
		{
			var transactionsList = new List<Transaction>()
			{
				new Transaction { CardId = 1, Id = 2, TransactionDate = DateTime.Now, TransactionTypeId = TransactionType.PayTrip.Id, TransactionAmount = 8m, PreviousBalance = 500m, NewBalance = 492m },
				new Transaction { CardId = 1, Id = 3, TransactionDate = DateTime.Now, TransactionTypeId = TransactionType.PayTrip.Id, TransactionAmount = 7.7m, PreviousBalance = 492m, NewBalance = 484.3m }
			};

			var result = cardTransactionProcessor.GetTripFare(transactionsList);

			Assert.Equal(7.7m, result);
		}

		[Fact]
		public void Return7Point7_GivenFiveTripTransactions()
		{
			var transactionsList = new List<Transaction>()
			{
				new Transaction { CardId = 1, Id = 2, TransactionDate = DateTime.Now, TransactionTypeId = TransactionType.PayTrip.Id, TransactionAmount = 8m, PreviousBalance = 500m, NewBalance = 492m },
				new Transaction { CardId = 1, Id = 3, TransactionDate = DateTime.Now, TransactionTypeId = TransactionType.PayTrip.Id, TransactionAmount = 7.7m, PreviousBalance = 492m, NewBalance = 484.3m },
				new Transaction { CardId = 1, Id = 4, TransactionDate = DateTime.Now, TransactionTypeId = TransactionType.PayTrip.Id, TransactionAmount = 7.7m, PreviousBalance = 484.3m, NewBalance = 476.6m },
				new Transaction { CardId = 1, Id = 5, TransactionDate = DateTime.Now, TransactionTypeId = TransactionType.PayTrip.Id, TransactionAmount = 7.7m, PreviousBalance = 476.6m, NewBalance = 468.9m },
				new Transaction { CardId = 1, Id = 6, TransactionDate = DateTime.Now, TransactionTypeId = TransactionType.PayTrip.Id, TransactionAmount = 7.7m, PreviousBalance = 468.9m, NewBalance = 461.2m },
			};

			var result = cardTransactionProcessor.GetTripFare(transactionsList);

			Assert.Equal(8m, result);
		}

		[Fact]
		public void Return8_GivenSixTripTransactions()
		{
			var transactionsList = new List<Transaction>()
			{
				new Transaction { CardId = 1, Id = 2, TransactionDate = DateTime.Now, TransactionTypeId = TransactionType.PayTrip.Id, TransactionAmount = 8m, PreviousBalance = 500m, NewBalance = 492m },
				new Transaction { CardId = 1, Id = 3, TransactionDate = DateTime.Now, TransactionTypeId = TransactionType.PayTrip.Id, TransactionAmount = 7.7m, PreviousBalance = 492m, NewBalance = 484.3m },
				new Transaction { CardId = 1, Id = 4, TransactionDate = DateTime.Now, TransactionTypeId = TransactionType.PayTrip.Id, TransactionAmount = 7.7m, PreviousBalance = 484.3m, NewBalance = 476.6m },
				new Transaction { CardId = 1, Id = 5, TransactionDate = DateTime.Now, TransactionTypeId = TransactionType.PayTrip.Id, TransactionAmount = 7.7m, PreviousBalance = 476.6m, NewBalance = 468.9m },
				new Transaction { CardId = 1, Id = 6, TransactionDate = DateTime.Now, TransactionTypeId = TransactionType.PayTrip.Id, TransactionAmount = 7.7m, PreviousBalance = 468.9m, NewBalance = 461.2m },
				new Transaction { CardId = 1, Id = 7, TransactionDate = DateTime.Now, TransactionTypeId = TransactionType.PayTrip.Id, TransactionAmount = 7.7m, PreviousBalance = 461.2m, NewBalance = 461.2m },
			};

			var result = cardTransactionProcessor.GetTripFare(transactionsList);

			Assert.Equal(8m, result);
		}
	}
}
