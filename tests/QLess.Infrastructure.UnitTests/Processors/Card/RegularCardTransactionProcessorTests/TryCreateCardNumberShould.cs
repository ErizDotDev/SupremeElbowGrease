using QLess.Infrastructure.Processors;
using Xunit;

namespace QLess.Infrastructure.UnitTests.Processors.Card.RegularCardTransactionProcessorTests
{
	public class TryCreateCardNumberShould
	{
		private const decimal MinimumRegularCardTypeLoad = 100m;
		private const decimal MaximumLoadTransactionAmount = 1000m;

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
		public void ReturnsEmptyString_GivenRegularCardTypeAndInitialBalanceLessThanMinLoadAmount()
		{
			decimal initialBalance = 99m;
			string expectedMessage = $"Minimum initial load balance not reached. Please load your card with at least P{MinimumRegularCardTypeLoad}.00";

			var result = cardTransactionProcessor.TryCreateCardNumber(initialBalance);

			Assert.True(string.IsNullOrEmpty(result.CardNumber));
			Assert.True(!string.IsNullOrEmpty(result.ErrorMessage));
			Assert.Equal(expectedMessage, result.ErrorMessage);
		}

		[Fact]
		public void ReturnsEmptyString_GivenRegularCardTypeAndInitialBalanceGreaterThanMinLoadAmount()
		{
			decimal initialBalance = 1001m;
			string expectedMessage = $"Exceeded max load amount per transaction P{MaximumLoadTransactionAmount}.00.";

			var result = cardTransactionProcessor.TryCreateCardNumber(initialBalance);

			Assert.True(string.IsNullOrEmpty(result.CardNumber));
			Assert.True(!string.IsNullOrEmpty(result.ErrorMessage));
			Assert.Equal(expectedMessage, result.ErrorMessage);
		}

		[Fact]
		public void ReturnCardNumber_GivenRegularCardTypeAndInitialBalanceBetweenMinAndMaxLoadAmount()
		{
			decimal initialBalance = 750m;

			var result = cardTransactionProcessor.TryCreateCardNumber(initialBalance);

			Assert.True(!string.IsNullOrEmpty(result.CardNumber));
			Assert.Contains(DateTime.Now.ToString("yyddMM"), result.CardNumber);
			Assert.True(string.IsNullOrEmpty(result.ErrorMessage));
		}
	}
}
