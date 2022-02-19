using QLess.Infrastructure.Processors;
using Xunit;

namespace QLess.Infrastructure.UnitTests.Processors.Card.DiscountedCardTransactionProcessorTests
{
	public class TryCreateCardNumberShould
	{
		private const decimal MinimumDiscountedCardTypeLoad = 500m;
		private const decimal MaximumLoadTransactionAmount = 1000m;

		private readonly BaseCardTransactionProcessor cardTransactionProcessor;

		public TryCreateCardNumberShould()
		{
			cardTransactionProcessor = new DiscountedCardTransactionProcessor();
		}

		[Fact]
		public void ReturnCardNumber_GivenDiscountedCardTypeValidIDNumberAndValidInitialBalance()
		{			
			decimal initialBalance = 500m;
			string specialIDNumber = "XXXXXXXXXX";

			var result = cardTransactionProcessor.TryCreateCardNumber(initialBalance, specialIDNumber);

			Assert.True(!string.IsNullOrEmpty(result.CardNumber));
			Assert.Contains(DateTime.Now.ToString("yyddMM"), result.CardNumber);
			Assert.True(string.IsNullOrEmpty(result.ErrorMessage));
		}

		[Fact]
		public void ReturnEmptyString_GivenDiscountedCardTypeInvalidIDNumberAndValidInitialBalance()
		{
			decimal initialBalance = 500m;
			string specialIDNumber = "XXXXXXXXXXX";
			string expectedMessage = "The provided ID number is invalid. Please provide a valid ID number. Valid ID numbers include yout Senior Citizen control number and your PWD ID number.";

			var result = cardTransactionProcessor.TryCreateCardNumber(initialBalance, specialIDNumber);

			Assert.True(string.IsNullOrEmpty(result.CardNumber));
			Assert.True(!string.IsNullOrEmpty(result.ErrorMessage));
			Assert.Equal(expectedMessage, result.ErrorMessage);
		}

		[Fact]
		public void ReturnEmptyString_GivenDiscountedCardTypeBlankIDNumberAndValidInitialBalance()
		{
			decimal initialBalance = 500m;
			string specialIDNumber = "";
			string expectedMessage = "The provided ID number is invalid. Please provide a valid ID number. Valid ID numbers include yout Senior Citizen control number and your PWD ID number.";

			var result = cardTransactionProcessor.TryCreateCardNumber(initialBalance, specialIDNumber);

			Assert.True(string.IsNullOrEmpty(result.CardNumber));
			Assert.True(!string.IsNullOrEmpty(result.ErrorMessage));
			Assert.Equal(expectedMessage, result.ErrorMessage);
		}

		[Fact]
		public void ReturnEmptyString_GivenDiscountedCardTypeValidIDNumberAndInitialBalanceLessThanMinLoadAmount()
		{
			decimal initialBalance = 499m;
			string specialIDNumber = "XXXXXXXXXXXX";
			string expectedMessage = $"Minimum initial load balance not reached. Please load your card with at least P{MinimumDiscountedCardTypeLoad}.00";

			var result = cardTransactionProcessor.TryCreateCardNumber(initialBalance, specialIDNumber);

			Assert.True(string.IsNullOrEmpty(result.CardNumber));
			Assert.True(!string.IsNullOrEmpty(result.ErrorMessage));
			Assert.Equal(expectedMessage, result.ErrorMessage);
		}

		[Fact]
		public void ReturnEmptyString_GivenDiscountedCardTypeValidIDNumberAndInitialBalanceGreaterThanMaxLoadAmount()
		{
			decimal initialBalance = 1001m;
			string specialIDNumber = "XXXXXXXXXXXX";
			string expectedMessage = $"Exceeded max load amount per transaction P{MaximumLoadTransactionAmount}.00.";

			var result = cardTransactionProcessor.TryCreateCardNumber(initialBalance, specialIDNumber);

			Assert.True(string.IsNullOrEmpty(result.CardNumber));
			Assert.True(!string.IsNullOrEmpty(result.ErrorMessage));
			Assert.Equal(expectedMessage, result.ErrorMessage);
		}

		[Fact]
		public void ReturnCardNumber_GivenDiscountedCardTypeValidIDNumberAndInitialBalanceBetweenMinAndMaxLoadAmount()
		{
			decimal initialBalance = 999m;
			string specialIDNumber = "XXXXXXXXXX";

			var result = cardTransactionProcessor.TryCreateCardNumber(initialBalance, specialIDNumber);

			Assert.True(!string.IsNullOrEmpty(result.CardNumber));
			Assert.Contains(DateTime.Now.ToString("yyddMM"), result.CardNumber);
			Assert.True(string.IsNullOrEmpty(result.ErrorMessage));
		}
	}
}
