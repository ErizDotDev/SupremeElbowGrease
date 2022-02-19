using NSubstitute;
using QLess.Core.Data;
using QLess.Core.Domain;
using QLess.Core.Enums;
using QLess.Core.Interface;
using QLess.Infrastructure.Services;
using Xunit;

namespace QLess.Infrastructure.UnitTests.Services.TripPaymentServiceTests
{
	public class PayForTripShould
	{
		private readonly ICardService _cardService;
		private readonly ITransactionService _transactionService;
		private readonly ITripPaymentService _tripPaymentService;

		public PayForTripShould()
		{
			_cardService = Substitute.For<ICardService>();
			_transactionService = Substitute.For<ITransactionService>();
			_tripPaymentService = new TripPaymentService(_cardService, _transactionService);
		}

		[Fact]
		public async Task ReturnFailedResponse_WithIdNotFoundScenario()
		{
			string cardNumber = "XXXXXXXXXXXX";

			string expectedMessage = "Card number is not found.";

			_cardService.FindCardDetailsByCardNumber(cardNumber).Returns<Card>(x => null);

			var result = await _tripPaymentService.PayForTrip(cardNumber);

			Assert.False(result.Succeeded);
			Assert.True(!string.IsNullOrEmpty(result.ErrorMessage));
			Assert.Equal(expectedMessage, result.ErrorMessage);
		}

		[Fact]
		public async Task ReturnFailedResponse_WithInsufficientBalanceInCard()
		{
			string cardNumber = "221802083101";

			var fakeCardDetail = new Card
			{
				CardTypeId = (int)CardType.Regular,
				CardNumber = "221802083101",
				Balance = 5m,
			};

			string expectedMessage = "Insufficient load balance. Please reload your card.";

			_cardService.FindCardDetailsByCardNumber(cardNumber).Returns<Card>(fakeCardDetail);

			var result = await _tripPaymentService.PayForTrip(cardNumber);

			Assert.False(result.Succeeded);
			Assert.True(!string.IsNullOrEmpty(result.ErrorMessage));
			Assert.Equal(expectedMessage, result.ErrorMessage);
		}

		[Fact]
		public async Task ReturnFailedResponse_WithInsufficientBalanceInCardVersusAssignedFare()
		{
			string cardNumber = "221802083101";

			var fakeCardDetail = new Card
			{
				CardTypeId = (int)CardType.Discounted,
				CardNumber = "221802083101",
				Balance = 5m,
			};

			var fakeRetrievedTransactions = new List<Transaction>()
			{
				new Transaction { CardId = 1, Id = 2, TransactionDate = DateTime.Now, TransactionTypeId = TransactionType.PayTrip.Id, TransactionAmount = 8m, PreviousBalance = 500m, NewBalance = 492m }
			};

			string expectedMessage = "Insufficient load balance. Please reload your card.";

			_cardService.FindCardDetailsByCardNumber(cardNumber).Returns<Card>(fakeCardDetail);
			_transactionService.GetTripTransactionsFromGivenDate(Arg.Any<long>(), DateTime.Now).ReturnsForAnyArgs(fakeRetrievedTransactions);

			var result = await _tripPaymentService.PayForTrip(cardNumber);

			Assert.False(result.Succeeded);
			Assert.True(!string.IsNullOrEmpty(result.ErrorMessage));
			Assert.Equal(expectedMessage, result.ErrorMessage);
		}

		[Fact]
		public async Task ReturnFailedResponse_WithSaveCardBalanceStepFailed()
		{
			string cardNumber = "221802083101";

			var fakeCardDetail = new Card
			{
				CardTypeId = (int)CardType.Discounted,
				CardNumber = "221802083101",
				Balance = 50m,
			};

			var fakeRetrievedTransactions = new List<Transaction>()
			{
				new Transaction { CardId = 1, Id = 2, TransactionDate = DateTime.Now, TransactionTypeId = TransactionType.PayTrip.Id, TransactionAmount = 8m, PreviousBalance = 500m, NewBalance = 492m }
			};

			string expectedMessage = "Failed to save new card balance. Please try again.";

			_cardService.FindCardDetailsByCardNumber(cardNumber).Returns<Card>(fakeCardDetail);
			_transactionService.GetTripTransactionsFromGivenDate(Arg.Any<long>(), DateTime.Now).ReturnsForAnyArgs(fakeRetrievedTransactions);
			_cardService.SaveNewCardBalance(fakeCardDetail, Arg.Any<decimal>()).ReturnsForAnyArgs(false);

			var result = await _tripPaymentService.PayForTrip(cardNumber);

			Assert.False(result.Succeeded);
			Assert.True(!string.IsNullOrEmpty(result.ErrorMessage));
			Assert.Equal(expectedMessage, result.ErrorMessage);
		}

		[Fact]
		public async Task ReturnFailedResponse_WithSaveTransactionStepFailed()
		{
			string cardNumber = "221802083101";

			var fakeCardDetail = new Card
			{
				CardTypeId = (int)CardType.Discounted,
				CardNumber = "221802083101",
				Balance = 50m,
			};

			var fakeRetrievedTransactions = new List<Transaction>()
			{
				new Transaction { CardId = 1, Id = 2, TransactionDate = DateTime.Now, TransactionTypeId = TransactionType.PayTrip.Id, TransactionAmount = 8m, PreviousBalance = 500m, NewBalance = 492m }
			};

			string expectedMessage = "Failed to save payment transaction.";

			_cardService.FindCardDetailsByCardNumber(cardNumber).Returns<Card>(fakeCardDetail);
			_transactionService.GetTripTransactionsFromGivenDate(Arg.Any<long>(), DateTime.Now).ReturnsForAnyArgs(fakeRetrievedTransactions);
			_cardService.SaveNewCardBalance(fakeCardDetail, Arg.Any<decimal>()).ReturnsForAnyArgs(true);
			_transactionService.SaveTripPaymentTransaction(fakeCardDetail, Arg.Any<decimal>()).ReturnsForAnyArgs(false);

			var result = await _tripPaymentService.PayForTrip(cardNumber);

			Assert.False(result.Succeeded);
			Assert.True(!string.IsNullOrEmpty(result.ErrorMessage));
			Assert.Equal(expectedMessage, result.ErrorMessage);
		}

		[Fact]
		public async Task ReturnSuccessResponse_WithValidAndAllInternalStepsPassed()
		{
			string cardNumber = "221802083101";

			var fakeCardDetail = new Card
			{
				CardTypeId = (int)CardType.Discounted,
				CardNumber = "221802083101",
				Balance = 50m,
			};

			var fakeRetrievedTransactions = new List<Transaction>()
			{
				new Transaction { CardId = 1, Id = 2, TransactionDate = DateTime.Now, TransactionTypeId = TransactionType.PayTrip.Id, TransactionAmount = 8m, PreviousBalance = 500m, NewBalance = 492m }
			};

			string expectedMessage = "Failed to save payment transaction.";

			_cardService.FindCardDetailsByCardNumber(cardNumber).Returns<Card>(fakeCardDetail);
			_transactionService.GetTripTransactionsFromGivenDate(Arg.Any<long>(), DateTime.Now).ReturnsForAnyArgs(fakeRetrievedTransactions);
			_cardService.SaveNewCardBalance(fakeCardDetail, Arg.Any<decimal>()).ReturnsForAnyArgs(true);
			_transactionService.SaveTripPaymentTransaction(fakeCardDetail, Arg.Any<decimal>()).ReturnsForAnyArgs(true);

			var result = await _tripPaymentService.PayForTrip(cardNumber);

			Assert.True(result.Succeeded);
			Assert.False(!string.IsNullOrEmpty(result.ErrorMessage));
		}
	}
}
