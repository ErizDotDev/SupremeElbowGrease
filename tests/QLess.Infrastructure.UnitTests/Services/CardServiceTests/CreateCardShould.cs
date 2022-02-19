using NSubstitute;
using QLess.Core.Domain;
using QLess.Core.Enums;
using QLess.Core.Interface;
using QLess.Infrastructure.Services;
using Xunit;

namespace QLess.Infrastructure.UnitTests.Services.CardServiceTests
{
	public class CreateCardShould
	{
		private readonly ICardRepository _cardRepository;
		private readonly ITransactionService _transactionService;
		private readonly ICardService _cardService;

		public CreateCardShould()
		{
			_cardRepository = Substitute.For<ICardRepository>();
			_transactionService = Substitute.For<ITransactionService>();
			_cardService = new CardService(_cardRepository, _transactionService);
		}

		[Fact]
		public async void ReturnFailedResponse_WithCreateCardInDbStepFailed()
		{
			var cardType = CardType.Regular;
			decimal initialBalance = 100m;

			string expectedMessage = "Failed to create card. Please try again.";

			_cardRepository.CreateAsync(Arg.Any<Card>()).ReturnsForAnyArgs(false);

			var result = await _cardService.CreateCard(cardType, initialBalance);

			Assert.True(string.IsNullOrEmpty(result.CardNumber));
			Assert.True(!string.IsNullOrEmpty(result.ErrorMessage));
			Assert.Equal(expectedMessage, result.ErrorMessage);
		}

		[Fact]
		public async void ReturnFailedResponse_WithCreateInitialLoadTransactionInDbStepFailed()
		{
			var cardType = CardType.Regular;
			decimal initialBalance = 100m;

			string expectedMessage = "Failed to save transaction.";

			_cardRepository.Create(Arg.Any<Card>(), out Arg.Any<long>()).ReturnsForAnyArgs(true);
			_transactionService.SaveCreateCardTransaction(Arg.Any<Card>()).ReturnsForAnyArgs(false);

			var result = await _cardService.CreateCard(cardType, initialBalance);

			Assert.True(string.IsNullOrEmpty(result.CardNumber));
			Assert.True(!string.IsNullOrEmpty(result.ErrorMessage));
			Assert.Equal(expectedMessage, result.ErrorMessage);
		}

		[Fact]
		public async void ReturnFailedResponse_GivenInvalidDataForRegularCardTypeCreation()
		{
			var cardType = CardType.Regular;
			decimal initialBalance = 50m;

			var result = await _cardService.CreateCard(cardType, initialBalance);

			Assert.True(string.IsNullOrEmpty(result.CardNumber));
			Assert.True(!string.IsNullOrEmpty(result.ErrorMessage));
		}

		[Fact]
		public async void ReturnFailedResponse_GivenInvalidDataForDiscountedCardTypeCreation()
		{
			var cardType = CardType.Discounted;
			decimal initialBalance = 1500m;
			string specialIdNumber = "XXXXXXXXXX";

			var result = await _cardService.CreateCard(cardType, initialBalance, specialIdNumber);

			Assert.True(string.IsNullOrEmpty(result.CardNumber));
			Assert.True(!string.IsNullOrEmpty(result.ErrorMessage));
		}

		[Fact]
		public async void ReturnSuccessResponse_GivenValidDataForRegularCardTypeCreation()
		{
			var cardType = CardType.Regular;
			decimal initialBalance = 200m;

			_cardRepository.Create(Arg.Any<Card>(), out Arg.Any<long>()).ReturnsForAnyArgs(true);
			_transactionService.SaveCreateCardTransaction(Arg.Any<Card>()).ReturnsForAnyArgs(true);

			var result = await _cardService.CreateCard(cardType, initialBalance);

			Assert.True(!string.IsNullOrEmpty(result.CardNumber));
			Assert.True(string.IsNullOrEmpty(result.ErrorMessage));
		}

		[Fact]
		public async void ReturnSuccessResponse_GivenValidDataForDiscountedCardTypeCreation()
		{
			var cardType = CardType.Discounted;
			decimal initialBalance = 800m;
			string specialIdNumber = "XXXXXXXXXX";

			_cardRepository.Create(Arg.Any<Card>(), out Arg.Any<long>()).ReturnsForAnyArgs(true);
			_transactionService.SaveCreateCardTransaction(Arg.Any<Card>()).ReturnsForAnyArgs(true);

			var result = await _cardService.CreateCard(cardType, initialBalance, specialIdNumber);

			Assert.True(!string.IsNullOrEmpty(result.CardNumber));
			Assert.True(string.IsNullOrEmpty(result.ErrorMessage));
		}
	}
}
