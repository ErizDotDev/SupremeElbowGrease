using QLess.Core.Domain;
using QLess.Core.Enums;
using QLess.Core.Interface;
using QLess.Infrastructure.Processors;

namespace QLess.Infrastructure.Services
{
	public class CardLoadService : ICardLoadService
	{
		private const decimal MinimumLoadTransactionAmount = 100m;
		private const decimal MaximumLoadTransactionAmount = 1000m;
		private const decimal MaximumLoadAmount = 10000m;
		
		private readonly ICardService _cardService;
		private readonly ITransactionService _transactionService;
		private Dictionary<CardType, Func<BaseCardTransactionProcessor>> cardTransactionProcessorList;

		public CardLoadService(ICardService cardService, ITransactionService transactionService)
		{
			_cardService = cardService;
			_transactionService = transactionService;
			cardTransactionProcessorList = BaseCardTransactionProcessor.GetAvailableTransactionProcessors();
		}

		public async Task<CardLoadResponse> LoadCard(string cardNumber, decimal loadAmount, decimal amountPaid)
		{
			if (loadAmount > amountPaid)
			{
				return new CardLoadResponse
				{ 
					Succeeded = false,
					Message = $"Invalid paid amount"
				};
			}

			if (loadAmount < MinimumLoadTransactionAmount)
			{
				return new CardLoadResponse
				{ 
					Succeeded = false,
					Message = $"Minimum load balance not reached. Please load your card with at least P{MinimumLoadTransactionAmount}.00"
				};
			}

			if (loadAmount > MaximumLoadTransactionAmount)
			{
				return new CardLoadResponse
				{ 
					Succeeded = false,
					Message = $"Exceeded max load amount per transaction P{MaximumLoadTransactionAmount}.00."
				};
			}

			var cardDetail = await _cardService.FindCardDetailsByCardNumber(cardNumber);
			if (cardDetail == null)
			{
				return new CardLoadResponse
				{
					Succeeded = false,
					Message = "Card number is not found."
				};
			}

			var cardTransactionProcessor = cardTransactionProcessorList[(CardType)cardDetail.CardTypeId];
			
			bool isCardExpired = cardTransactionProcessor.Invoke().IsCardExpired(cardDetail.DateLastUsed.Value, DateTime.Now);
			if (isCardExpired)
			{
				return new CardLoadResponse
				{
					Succeeded = false,
					Message = "Card is already expired. Please create another card."
				};
			}

			if (cardDetail.Balance >= MaximumLoadAmount)
			{
				return new CardLoadResponse
				{
					Succeeded = false,
					Message = $"Your load wallet still has the maximum load amount P{MaximumLoadAmount}.00."
				};
			}

			var paymentDetails = ComputePaymentDetails(cardDetail.Balance, loadAmount, amountPaid);

			bool isSaveCardBalanceSuccess = await _cardService.SaveNewCardBalance(cardDetail, paymentDetails.CardBalance);			
			if (!isSaveCardBalanceSuccess)
			{
				return new CardLoadResponse
				{
					Succeeded = false,
					Message = "Failed to save new card balance. Please try again."
				};
			}

			bool isSaveTransactionSuccess = await _transactionService.SaveLoadCardTransaction(cardDetail, paymentDetails);
			if (!isSaveTransactionSuccess)
			{
				return new CardLoadResponse
				{
					Succeeded = false,
					Message = "Failed to save payment transaction."
				};
			}

			return new CardLoadResponse
			{ 
				PaymentDetail = paymentDetails,
				Succeeded = true
			};
		}

		private LoadPaymentDetail ComputePaymentDetails(decimal cardBalance, decimal loadAmount, decimal amountPaid)
		{
			decimal newBalance = cardBalance + loadAmount;
			decimal change = amountPaid - loadAmount;
			decimal surplus = 0m;

			if (newBalance > MaximumLoadAmount)
			{
				surplus = newBalance - MaximumLoadAmount;
				newBalance -= surplus;
				change += surplus;
			}

			return new LoadPaymentDetail
			{ 
				LoadAmount = loadAmount,
				AmountPaid = amountPaid,
				Change = change,
				CardBalance = newBalance
			};
		}
	}
}
