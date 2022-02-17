using QLess.Core.Domain;
using QLess.Core.Enums;
using QLess.Core.Interface;

namespace QLess.Core.Services
{
	public class CardService : ICardService
	{
		public async Task<CreateCardResponse> CreateCard(CardType cardType, decimal initialBalance)
		{
			string cardNumber = string.Empty;

			switch (cardType)
			{
				case CardType.Regular:
					return await Task.FromResult(CreateRegularCard(initialBalance));
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

		private string GenerateCardNumber()
		{
			return DateTime.Now.ToString("yyddMMhhmmss");
		}
	}
}
