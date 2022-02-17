using QLess.Core.Domain;

namespace QLess.Infrastructure.Processors
{
	public class DiscountedCardTransactionProcessor : BaseCardTransactionProcessor
	{
		public override CreateCardResponse CreateCardDetails(decimal initialBalance, string specialIdNumber = "")
		{
			const int minimumDiscountedCardTypeLoad = 500;

			if (initialBalance < minimumDiscountedCardTypeLoad)
			{
				return new CreateCardResponse
				{
					CardNumber = string.Empty,
					ErrorMessage = $"Minimum initial load balance not reached. Please load your card with at least P{minimumDiscountedCardTypeLoad}.00"
				};
			}

			if (!IsSpecialIdValid(specialIdNumber))
			{
				return new CreateCardResponse
				{
					CardNumber = string.Empty,
					ErrorMessage = $"The provided ID number is invalid. Please provide a valid ID number. Valid ID numbers include yout Senior Citizen control number and your PWD ID number."
				};
			}

			string cardNumber = GenerateCardNumber();

			return new CreateCardResponse { CardNumber = cardNumber };
		}

		private bool IsSpecialIdValid(string specialIdNumber)
		{
			return !string.IsNullOrEmpty(specialIdNumber)
				&& (specialIdNumber.Length == 10 || specialIdNumber.Length == 12);
		}
	}
}
