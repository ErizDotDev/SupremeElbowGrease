using QLess.Core.Enums;
using QLess.Core.Interface;
using QLess.Core.Services;
using Xunit;

namespace QLess.UnitTests
{
	public class CardServiceTests
	{
		private readonly ICardService _cardService = new CardService(); 

		[Fact]
		public async Task CreateCard_ReturnsCardNumber_GivenRegularCardTypeAndInitialBalance()
		{
			CardType cardType = CardType.Regular;
			decimal initialBalance = 100m;

			var result = await _cardService.CreateCard(cardType, initialBalance);

			Assert.True(!string.IsNullOrEmpty(result.CardNumber));
			Assert.Contains(DateTime.Now.ToString("yyddMM"), result.CardNumber);
			Assert.True(string.IsNullOrEmpty(result.ErrorMessage));
		}

		[Fact]
		public async Task CreateCard_ReturnsEmptyString_GivenRegularCardTypeAndInvalidInitialBalance()
		{
			CardType cardType = CardType.Regular;
			decimal initialBalance = 0m;

			var result = await _cardService.CreateCard(cardType, initialBalance);

			Assert.True(string.IsNullOrEmpty(result.CardNumber));
			Assert.True(!string.IsNullOrEmpty(result.ErrorMessage));
		}
	}
}