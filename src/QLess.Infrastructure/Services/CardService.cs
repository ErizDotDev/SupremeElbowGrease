using QLess.Core.Data;
using QLess.Core.Domain;
using QLess.Core.Enums;
using QLess.Core.Interface;
using QLess.Infrastructure.Processors;

namespace QLess.Infrastructure.Services
{
	public class CardService : ICardService
	{
		private readonly ICardRepository _cardRepository;
		private readonly ITransactionService _transactionService;
		private Dictionary<CardType, Func<BaseCardTransactionProcessor>> cardTransactionProcessorList;

		public CardService(ICardRepository cardRepository, ITransactionService transactionService)
		{
			_cardRepository = cardRepository;
			_transactionService = transactionService;
			cardTransactionProcessorList = BaseCardTransactionProcessor.GetAvailableTransactionProcessors();
		}

		public async Task<CreateCardResponse> CreateCard(CardType cardType, decimal initialBalance, string specialIdNumber = "")
		{
			var cardTransactionProcessor = cardTransactionProcessorList[cardType];
			
			var processResponse = cardTransactionProcessor.Invoke().TryCreateCardNumber(initialBalance, specialIdNumber);
			if (!string.IsNullOrEmpty(processResponse.ErrorMessage))
				return processResponse;

			var cardDetail = new Card
			{ 
				CardTypeId = (int)cardType,
				CardNumber = processResponse.CardNumber,
				Balance = initialBalance
			};

			long cardId = 0;
			bool isCreateCardSuccess = _cardRepository.Create(cardDetail, out cardId);
			if (!isCreateCardSuccess)
			{
				return new CreateCardResponse
				{
					CardNumber = string.Empty,
					ErrorMessage = "Failed to create card. Please try again."
				};
			}

			cardDetail.Id = cardId;
			bool isSaveCreateCardTransactionSuccess = await _transactionService.SaveCreateCardTransaction(cardDetail);
			if (!isSaveCreateCardTransactionSuccess)
			{
				return new CreateCardResponse
				{
					CardNumber = string.Empty,
					ErrorMessage = "Failed to create card. Please try again."
				};
			}

			return processResponse;
		}

		public async Task<bool> SaveNewCardBalance(Card cardDetail, decimal fare)
		{
			cardDetail.Balance = cardDetail.Balance - fare;
			return await _cardRepository.UpdateAsync(cardDetail);
		}

		public async Task<Card> FindCardDetailsByCardNumber(string cardNumber)
			=> await Task.FromResult(_cardRepository.FindByCardNumber(cardNumber));
	}
}
