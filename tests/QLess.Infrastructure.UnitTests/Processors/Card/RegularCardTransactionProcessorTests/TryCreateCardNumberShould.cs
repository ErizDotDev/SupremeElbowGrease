using QLess.Infrastructure.Processors;
using Xunit;

namespace QLess.Infrastructure.UnitTests.Processors.Card.RegularCardTransactionProcessorTests
{
	public class TryCreateCardNumberShould
	{
		private readonly BaseCardTransactionProcessor cardTransactionProcessor;

		public TryCreateCardNumberShould()
		{
			cardTransactionProcessor = new RegularCardTransactionProcessor();
		}

		[Fact]
		public void ReturnCardNumber_GivenRegularCardTypeAndValidInitialBalance()
		{
			decimal initialBalance = 100m;

			var result = cardTransactionProcessor.TryCreateCardNumber(initialBalance);

			Assert.True(!string.IsNullOrEmpty(result.CardNumber));
			Assert.Contains(DateTime.Now.ToString("yyddMM"), result.CardNumber);
			Assert.True(string.IsNullOrEmpty(result.ErrorMessage));
		}

		[Fact]
		public void ReturnsEmptyString_GivenRegularCardTypeAndInvalidInitialBalance()
		{
			decimal initialBalance = 99m;

			var result = cardTransactionProcessor.TryCreateCardNumber(initialBalance);

			Assert.True(string.IsNullOrEmpty(result.CardNumber));
			Assert.True(!string.IsNullOrEmpty(result.ErrorMessage));
		}
	}
}
