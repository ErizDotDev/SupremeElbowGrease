using QLess.Core.Domain;
using QLess.Core.Enums;
using QLess.Core.Interface;
using QLess.Infrastructure.Processors;

namespace QLess.Infrastructure.Services
{
	public class CardService : ICardService
	{
		private Dictionary<CardType, Func<BaseCardTransactionProcessor>> cardTransactionProcessorList = new Dictionary<CardType, Func<BaseCardTransactionProcessor>>()
		{ 
			{ CardType.Regular, () => new RegularCardTransactionProcessor() },
			{ CardType.Discounted, () => new DiscountedCardTransactionProcessor() }
		};

		public async Task<CreateCardResponse> CreateCard(CardType cardType, decimal initialBalance, string specialIdNumber = "")
		{
			var cardTransactionProcessor = cardTransactionProcessorList[cardType];
			return await Task.FromResult(cardTransactionProcessor.Invoke().CreateCardDetails(initialBalance, specialIdNumber));
		}
	}
}
