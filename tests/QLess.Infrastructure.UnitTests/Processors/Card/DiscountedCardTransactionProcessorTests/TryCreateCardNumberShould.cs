using QLess.Infrastructure.Processors;
using Xunit;

namespace QLess.Infrastructure.UnitTests.Processors.Card.DiscountedCardTransactionProcessorTests
{
	public class TryCreateCardNumberShould
	{
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

			var result = cardTransactionProcessor.TryCreateCardNumber(initialBalance, specialIDNumber);

			Assert.True(string.IsNullOrEmpty(result.CardNumber));
			Assert.True(!string.IsNullOrEmpty(result.ErrorMessage));
		}

		[Fact]
		public void ReturnEmptyString_GivenDiscountedCardTypeBlankIDNumberAndValidInitialBalance()
		{
			decimal initialBalance = 500m;
			string specialIDNumber = "";

			var result = cardTransactionProcessor.TryCreateCardNumber(initialBalance, specialIDNumber);

			Assert.True(string.IsNullOrEmpty(result.CardNumber));
			Assert.True(!string.IsNullOrEmpty(result.ErrorMessage));
		}

		[Fact]
		public void ReturnEmptyString_GivenDiscountedCardTypeValidIDNumberAndInvalidInitialBalance()
		{
			decimal initialBalance = 499m;
			string specialIDNumber = "XXXXXXXXXXXX";

			var result = cardTransactionProcessor.TryCreateCardNumber(initialBalance, specialIDNumber);

			Assert.True(string.IsNullOrEmpty(result.CardNumber));
			Assert.True(!string.IsNullOrEmpty(result.ErrorMessage));
		}
	}
}
