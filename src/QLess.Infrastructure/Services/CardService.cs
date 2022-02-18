using QLess.Core.Data;
using QLess.Core.Domain;
using QLess.Core.Enums;
using QLess.Core.Interface;
using QLess.Infrastructure.Processors;

namespace QLess.Infrastructure.Services
{
	public class CardService : ICardService
	{
		private readonly IRepository<Card> _cardRepository;
		private readonly IRepository<Transaction> _transactionRepository;
		private Dictionary<CardType, Func<BaseCardTransactionProcessor>> cardTransactionProcessorList;

		public CardService(IRepository<Card> cardRepository, IRepository<Transaction> transactionRepository)
		{
			_cardRepository = cardRepository;
			_transactionRepository = transactionRepository;
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

			bool isCreateSuccess = await CreateCardRecord(cardDetail);
			if (!isCreateSuccess)
			{
				return new CreateCardResponse
				{
					CardNumber = string.Empty,
					ErrorMessage = "Failed to create card. Please try again."
				};
			}

			return processResponse;
		}

		private async Task<bool> CreateCardRecord(Card cardDetail)
		{
			bool result = true;
			long cardId = 0;

			result = result && _cardRepository.Create(cardDetail, out cardId);
			
			cardDetail.Id = cardId;
			result = result && await SaveCreateCardTransaction(cardDetail);

			return result;
		}

		private async Task<bool> SaveCreateCardTransaction(Card cardDetail)
		{
			var newTransaction = new Transaction
			{ 
				CardId = cardDetail.Id,
				TransactionDate = DateTime.Now,
				TransactionTypeId = TransactionType.InitialLoad.Id,
				TransactionAmount = cardDetail.Balance,
				PreviousBalance = 0,
				NewBalance = cardDetail.Balance
			};

			return await _transactionRepository.CreateAsync(newTransaction);
		}
	}
}
