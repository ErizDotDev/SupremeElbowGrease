using QLess.Infrastructure.Processors;
using Xunit;

namespace QLess.Infrastructure.UnitTests.Processors.Card.DiscountedCardTransactionProcessorTests
{
	public class IsCardExpiredShould
	{
		private readonly DiscountedCardTransactionProcessor cardTransactionProcessor;

		public IsCardExpiredShould()
		{
			cardTransactionProcessor = new DiscountedCardTransactionProcessor();
		}

		[Fact]
		public void ReturnFalse_GivenDateLastUsedIsWithinTheCardExpirationPeriod()
		{
			DateTime dateLastUsed = DateTime.Now;
			DateTime dateUsed = DateTime.Now.AddYears(2);

			bool result = cardTransactionProcessor.IsCardExpired(dateLastUsed, dateUsed);

			Assert.False(result);
		}

		[Fact]
		public void ReturnTrue_GivenDateLastUsedIsExceededTheCardExpirationPeriod()
		{
			DateTime dateLastUsed = DateTime.Now;
			DateTime dateUsed = DateTime.Now.AddYears(5).AddDays(1);

			bool result = cardTransactionProcessor.IsCardExpired(dateLastUsed, dateUsed);

			Assert.True(result);
		}
	}
}
