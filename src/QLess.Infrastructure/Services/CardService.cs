using QLess.Core.Domain;
using QLess.Core.Enums;
using QLess.Core.Interface;

namespace QLess.Infrastructure.Services
{
	public class CardService : ICardService
	{
		public async Task<CreateCardResponse> CreateCard(CardType cardType, decimal initialBalance, string specialIdNumber = "")
		{
			string cardNumber = string.Empty;

			switch (cardType)
			{
				case CardType.Regular:
					return await Task.FromResult(CreateRegularCard(initialBalance));
				case CardType.Discounted:
					return await Task.FromResult(CreateDiscountedCard(initialBalance, specialIdNumber));
			}

			return await Task.FromResult(new CreateCardResponse());
		}

		private CreateCardResponse CreateRegularCard(decimal initialBalance)
		{
			const int minimumRegularCardTypeLoad = 100;

			if (initialBalance < minimumRegularCardTypeLoad)
			{
				return new CreateCardResponse
				{
					CardNumber = string.Empty,
					ErrorMessage = $"Minimum initial load balance not reached. Please load your card with at least P{minimumRegularCardTypeLoad}.00"
				};
			}

			string cardNumber = GenerateCardNumber();

			return new CreateCardResponse { CardNumber = cardNumber };
		}

		private CreateCardResponse CreateDiscountedCard(decimal initialBalance, string specialIdNumber)
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
					ErrorMessage = $"The provided ID number is invalid. Please provide a valid ID number. Valid ID numbers include the Senior Citizen control number and PWD ID number."
				};
			}

			string cardNumber = GenerateCardNumber();

			return new CreateCardResponse { CardNumber = cardNumber };
		}

		private string GenerateCardNumber()
		{
			return DateTime.Now.ToString("yyddMMhhmmss");
		}

		private bool IsSpecialIdValid(string specialIdNumber)
		{
			return !string.IsNullOrEmpty(specialIdNumber)
				&& (specialIdNumber.Length == 10 || specialIdNumber.Length == 12);
		}
	}
}
