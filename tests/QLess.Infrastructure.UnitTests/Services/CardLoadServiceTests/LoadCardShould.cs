using NSubstitute;
using QLess.Core.Domain;
using QLess.Core.Enums;
using QLess.Core.Interface;
using QLess.Infrastructure.Services;
using Xunit;

namespace QLess.Infrastructure.UnitTests.Services.CardLoadServiceTests
{
	public class LoadCardShould
	{
		private const decimal MinimumLoadTransactionAmount = 100m;
		private const decimal MaximumLoadTransactionAmount = 1000m;
		private const decimal MaximumLoadAmount = 10000m;

		private readonly ICardService _cardService;
		private readonly ITransactionService _transactionService;
		private readonly ICardLoadService _cardLoadService;

		public LoadCardShould()
		{
			_cardService = Substitute.For<ICardService>();
			_transactionService = Substitute.For<ITransactionService>();
			_cardLoadService = new CardLoadService(_cardService, _transactionService);
		}

		[Fact]
		public async Task ReturnFailedResponse_GivenInvalidPaidAmount()
		{
			string cardNumber = "XXXXXXXXXXXX";
			decimal loadAmount = 500m;
			decimal amountPaid = 100m;

			string expectedMessage = "Invalid paid amount";

			var result = await _cardLoadService.LoadCard(cardNumber, loadAmount, amountPaid);

			Assert.False(result.Succeeded);
			Assert.True(!string.IsNullOrEmpty(result.Message));
			Assert.Equal(expectedMessage, result.Message);
		}

		[Fact]
		public async Task ReturnFailedResponse_GivenLoadAmountBelowSetMinimumLoadAmount()
		{
			string cardNumber = "XXXXXXXXXXXX";
			decimal loadAmount = 99m;
			decimal amountPaid = 100m;

			var expectedMessage = $"Minimum initial load balance not reached. Please load your card with at least P{MinimumLoadTransactionAmount}.00";

			var result = await _cardLoadService.LoadCard(cardNumber, loadAmount, amountPaid);

			Assert.False(result.Succeeded);
			Assert.True(!string.IsNullOrEmpty(result.Message));
			Assert.Equal(expectedMessage, result.Message);
		}

		[Fact]
		public async Task ReturnFailedResponse_GivenLoadAmountExceedingSetMaximumLoadAmount()
		{
			string cardNumber = "XXXXXXXXXXXX";
			decimal loadAmount = 1001m;
			decimal amountPaid = 1500m;

			var expectedMessage = $"Exceeded max load amount per transaction P{MaximumLoadTransactionAmount}.00.";

			var result = await _cardLoadService.LoadCard(cardNumber, loadAmount, amountPaid);

			Assert.False(result.Succeeded);
			Assert.True(!string.IsNullOrEmpty(result.Message));
			Assert.Equal(expectedMessage, result.Message);
		}

		[Fact]
		public async Task ReturnFailedResponse_WithCardDetailNotFoundWithGivenId()
		{
			string cardNumber = "XXXXXXXXXXXX";
			decimal loadAmount = 500m;
			decimal amountPaid = 1000m;

			var expectedMessage = "Card number is not found.";

			_cardService.FindCardDetailsByCardNumber(cardNumber).Returns<Card>(x => null);

			var result = await _cardLoadService.LoadCard(cardNumber, loadAmount, amountPaid);

			Assert.False(result.Succeeded);
			Assert.True(!string.IsNullOrEmpty(result.Message));
			Assert.Equal(expectedMessage, result.Message);
		}

		[Fact]
		public async Task ReturnFailedResponse_WithMaximumAmountAsBalanceInCard()
		{
			string cardNumber = "221802083101";
			decimal loadAmount = 500m;
			decimal amountPaid = 1000m;

			var fakeCardDetail = new Card
			{
				CardTypeId = (int)CardType.Regular,
				CardNumber = "221802083101",
				Balance = 10000m,
			};

			var expectedMessage = $"Your load wallet still has the maximum load amount P{MaximumLoadAmount}.00.";

			_cardService.FindCardDetailsByCardNumber(cardNumber).Returns<Card>(fakeCardDetail);

			var result = await _cardLoadService.LoadCard(cardNumber, loadAmount, amountPaid);

			Assert.False(result.Succeeded);
			Assert.True(!string.IsNullOrEmpty(result.Message));
			Assert.Equal(expectedMessage, result.Message);
		}

		[Fact]
		public async Task ReturnFailedResponse_WithSaveCardBalanceStepFailed()
		{
			string cardNumber = "221802083101";
			decimal loadAmount = 500m;
			decimal amountPaid = 1000m;

			var fakeCardDetail = new Card
			{
				CardTypeId = (int)CardType.Regular,
				CardNumber = "221802083101",
				Balance = 100m,
			};

			var expectedMessage = "Failed to save new card balance. Please try again.";

			_cardService.FindCardDetailsByCardNumber(cardNumber).Returns<Card>(fakeCardDetail);
			_cardService.SaveNewCardBalance(fakeCardDetail, Arg.Any<decimal>()).ReturnsForAnyArgs(false);

			var result = await _cardLoadService.LoadCard(cardNumber, loadAmount, amountPaid);

			Assert.False(result.Succeeded);
			Assert.True(!string.IsNullOrEmpty(result.Message));
			Assert.Equal(expectedMessage, result.Message);
		}

		[Fact]
		public async Task ReturnFailedResponse_WithSaveLoadCardBalanceStepFailed()
		{
			string cardNumber = "221802083101";
			decimal loadAmount = 500m;
			decimal amountPaid = 1000m;

			var fakeCardDetail = new Card
			{
				CardTypeId = (int)CardType.Regular,
				CardNumber = "221802083101",
				Balance = 100m,
			};

			var expectedMessage = "Failed to save payment transaction.";

			_cardService.FindCardDetailsByCardNumber(cardNumber).Returns<Card>(fakeCardDetail);
			_cardService.SaveNewCardBalance(fakeCardDetail, Arg.Any<decimal>()).ReturnsForAnyArgs(true);
			_transactionService.SaveLoadCardTransaction(fakeCardDetail, Arg.Any<LoadPaymentDetail>()).ReturnsForAnyArgs(false);

			var result = await _cardLoadService.LoadCard(cardNumber, loadAmount, amountPaid);

			Assert.False(result.Succeeded);
			Assert.True(!string.IsNullOrEmpty(result.Message));
			Assert.Equal(expectedMessage, result.Message);
		}

		// Customer has P100 balance in their transport card
		// Amount to Load: 500
		// Customer Money: 1000
		// Change: 500
		// New Balance: 600
		[Fact]
		public async Task ReturnSuccessResponse_WithAllValidDataAndAllStepsSucceed()
		{
			string cardNumber = "221802083101";
			decimal loadAmount = 500m;
			decimal amountPaid = 1000m;

			var fakeCardDetail = new Card
			{
				CardTypeId = (int)CardType.Regular,
				CardNumber = "221802083101",
				Balance = 100m,
			};

			_cardService.FindCardDetailsByCardNumber(cardNumber).Returns<Card>(fakeCardDetail);
			_cardService.SaveNewCardBalance(fakeCardDetail, Arg.Any<decimal>()).ReturnsForAnyArgs(true);
			_transactionService.SaveLoadCardTransaction(fakeCardDetail, Arg.Any<LoadPaymentDetail>()).ReturnsForAnyArgs(true);

			var result = await _cardLoadService.LoadCard(cardNumber, loadAmount, amountPaid);

			Assert.True(result.Succeeded);
			Assert.Equal(loadAmount, result.PaymentDetail.LoadAmount);
			Assert.Equal(amountPaid, result.PaymentDetail.AmountPaid);
			Assert.Equal(600, result.PaymentDetail.CardBalance);
			Assert.Equal(500, result.PaymentDetail.Change);
		}

		// Customer has P9500 balance in their transport card
		// Amount to Load: 1000
		// Customer Money: 1000
		// Change: 500
		// New Balance: 10000
		[Fact]
		public async Task ReturnSuccessResponse_WithAllValidDataAndCoveringFirstTestScenario()
		{
			string cardNumber = "221802083101";
			decimal loadAmount = 1000m;
			decimal amountPaid = 1000m;

			var fakeCardDetail = new Card
			{
				CardTypeId = (int)CardType.Regular,
				CardNumber = "221802083101",
				Balance = 9500m,
			};

			_cardService.FindCardDetailsByCardNumber(cardNumber).Returns<Card>(fakeCardDetail);
			_cardService.SaveNewCardBalance(fakeCardDetail, Arg.Any<decimal>()).ReturnsForAnyArgs(true);
			_transactionService.SaveLoadCardTransaction(fakeCardDetail, Arg.Any<LoadPaymentDetail>()).ReturnsForAnyArgs(true);

			var result = await _cardLoadService.LoadCard(cardNumber, loadAmount, amountPaid);

			Assert.True(result.Succeeded);
			Assert.Equal(loadAmount, result.PaymentDetail.LoadAmount);
			Assert.Equal(amountPaid, result.PaymentDetail.AmountPaid);
			Assert.Equal(10000m, result.PaymentDetail.CardBalance);
			Assert.Equal(500m, result.PaymentDetail.Change);
		}
	}
}
