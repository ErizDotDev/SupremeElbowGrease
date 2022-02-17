using QLess.Core.Enums;
using QLess.Core.Interface;
using QLess.Infrastructure.Services;
using Xunit;

namespace QLess.UnitTests
{
	public class CardServiceTests
	{
		private readonly ICardService _cardService = new CardService(); 

		[Fact]
		public async Task CreateCard_ReturnsCardNumber_GivenRegularCardTypeAndValidInitialBalance()
		{
			var cardType = CardType.Regular;
			decimal initialBalance = 100m;

			var result = await _cardService.CreateCard(cardType, initialBalance);

			Assert.True(!string.IsNullOrEmpty(result.CardNumber));
			Assert.Contains(DateTime.Now.ToString("yyddMM"), result.CardNumber);
			Assert.True(string.IsNullOrEmpty(result.ErrorMessage));
		}

		[Fact]
		public async Task CreateCard_ReturnsEmptyString_GivenRegularCardTypeAndInvalidInitialBalance()
		{
			var cardType = CardType.Regular;
			decimal initialBalance = 99m;

			var result = await _cardService.CreateCard(cardType, initialBalance);

			Assert.True(string.IsNullOrEmpty(result.CardNumber));
			Assert.True(!string.IsNullOrEmpty(result.ErrorMessage));
		}

		[Fact]
		public async Task CreateCard_ReturnsCardNumber_GivenDiscountedCardTypeValidIDNumberAndValidInitialBalance()
		{
			var cardType = CardType.Discounted;
			decimal initialBalance = 500m;
			string specialIDNumber = "XXXXXXXXXX";

			var result = await _cardService.CreateCard(cardType, initialBalance, specialIDNumber);

			Assert.True(!string.IsNullOrEmpty(result.CardNumber));
			Assert.Contains(DateTime.Now.ToString("yyddMM"), result.CardNumber);
			Assert.True(string.IsNullOrEmpty(result.ErrorMessage));
		}

		[Fact]
		public async Task CreateCard_ReturnsEmptyString_GivenDiscountedCardTypeInvalidIDNumberAndValidInitialBalance()
		{
			var cardType = CardType.Discounted;
			decimal initialBalance = 500m;
			string specialIDNumber = "XXXXXXXXXXX";

			var result = await _cardService.CreateCard(cardType, initialBalance, specialIDNumber);

			Assert.True(string.IsNullOrEmpty(result.CardNumber));
			Assert.True(!string.IsNullOrEmpty(result.ErrorMessage));
		}

		[Fact]
		public async Task CreateCard_ReturnsEmptyString_GivenDiscountedCardTypeBlankIDNumberAndValidInitialBalance()
		{
			var cardType = CardType.Discounted;
			decimal initialBalance = 500m;
			string specialIDNumber = "";

			var result = await _cardService.CreateCard(cardType, initialBalance, specialIDNumber);

			Assert.True(string.IsNullOrEmpty(result.CardNumber));
			Assert.True(!string.IsNullOrEmpty(result.ErrorMessage));
		}

		[Fact]
		public async Task CreateCard_ReturnsEmptyString_GivenDiscountedCardTypeValidIDNumberAndInvalidInitialBalance()
		{
			var cardType = CardType.Discounted;
			decimal initialBalance = 499m;
			string specialIDNumber = "XXXXXXXXXXXX";

			var result = await _cardService.CreateCard(cardType, initialBalance, specialIDNumber);

			Assert.True(string.IsNullOrEmpty(result.CardNumber));
			Assert.True(!string.IsNullOrEmpty(result.ErrorMessage));
		}
	}
}