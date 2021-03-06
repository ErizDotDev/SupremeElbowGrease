using QLess.Core.Domain;
using QLess.Core.Helpers;

namespace QLess.Infrastructure.Processors
{
	public class DiscountedCardTransactionProcessor : BaseCardTransactionProcessor
	{
		private const int YearsBeforeExpiry = 3;

		public override CreateCardResponse TryCreateCardNumber(decimal initialLoadAmount, string specialIdNumber = "")
		{
			const decimal minimumDiscountedCardTypeLoad = 500;

			if (initialLoadAmount < minimumDiscountedCardTypeLoad)
			{
				return new CreateCardResponse
				{
					CardNumber = string.Empty,
					ErrorMessage = $"Minimum initial load balance not reached. Please load your card with at least P{minimumDiscountedCardTypeLoad}.00"
				};
			}

			if (initialLoadAmount > MaximumLoadTransactionAmount)
			{
				return new CreateCardResponse
				{
					CardNumber = string.Empty,
					ErrorMessage = $"Exceeded max load amount per transaction P{MaximumLoadTransactionAmount}.00."
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

		public override decimal GetTripFare(List<Transaction> currentDateTripTransactions)
		{
			const decimal baseTripFare = 10m;
			const decimal baseDiscount = 0.2m;
			const decimal additionalDiscount = 0.03m;

			if (currentDateTripTransactions == null || currentDateTripTransactions.Count < 1
				|| currentDateTripTransactions.Count >= 5)
				return baseTripFare - (baseTripFare * baseDiscount);

			return baseTripFare - (baseTripFare * (baseDiscount + additionalDiscount));
		}

		public override bool IsCardExpired(DateTime dateLastUsed, DateTime dateUsed)
			=> dateLastUsed.GetDayStartDateTime().AddYears(YearsBeforeExpiry) < dateUsed;

		private bool IsSpecialIdValid(string specialIdNumber)
		{
			return !string.IsNullOrEmpty(specialIdNumber)
				&& (specialIdNumber.Length == 10 || specialIdNumber.Length == 12);
		}
	}
}
