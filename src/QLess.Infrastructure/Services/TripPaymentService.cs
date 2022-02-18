using QLess.Core.Enums;
using QLess.Core.Interface;
using QLess.Core.Services;
using QLess.Infrastructure.Processors;

namespace QLess.Infrastructure.Services
{
	public class TripPaymentService : ITripPaymentService
	{
		private readonly ICardService _cardService;
		private readonly ITransactionService _transactionService;
		private Dictionary<CardType, Func<BaseCardTransactionProcessor>> cardTransactionProcessorList;

		public TripPaymentService(ICardService cardService, ITransactionService transactionService)
		{
			_cardService = cardService;
			_transactionService = transactionService;
			cardTransactionProcessorList = BaseCardTransactionProcessor.GetAvailableTransactionProcessors();
		}

		public async Task<ServiceResponse> PayForTrip(string cardNumber)
		{
			var cardDetail = await _cardService.FindCardDetailsByCardNumber(cardNumber);

			if (cardDetail == null)
			{
				return new ServiceResponse
				{ 
					Succeeded = false,
					ErrorMessage = "Card number is not found."
				};
			}

			if (cardDetail.Balance <= 0)
			{
				return new ServiceResponse
				{
					Succeeded = false,
					ErrorMessage = "Insufficient load balance. Please reload your card."
				};
			}			

			var cardTransactionProcessor = cardTransactionProcessorList[(CardType)cardDetail.CardTypeId];

			var currentDateTripTransactions = await _transactionService.GetTripTransactionsFromGivenDate(DateTime.Now);
			decimal tripFare = cardTransactionProcessor.Invoke().GetTripFare(currentDateTripTransactions);
			decimal newCardBalance = cardDetail.Balance - tripFare;

			if (newCardBalance <= 0)
			{
				return new ServiceResponse
				{
					Succeeded = false,
					ErrorMessage = "Insufficient load balance. Please reload your card."
				};
			}

			bool isSaveCardBalanceSuccess = await _cardService.SaveNewCardBalance(cardDetail, newCardBalance);
			if (!isSaveCardBalanceSuccess)
			{
				return new ServiceResponse
				{
					Succeeded = false,
					ErrorMessage = "Failed to save new card balance. Please try again."
				};
			}

			bool isSavePaymentTransactionSuccess = await _transactionService.SaveTripPaymentTransaction(cardDetail, tripFare);
			if (!isSavePaymentTransactionSuccess)
			{
				return new ServiceResponse
				{
					Succeeded = false,
					ErrorMessage = "Failed to save payment transaction."
				};
			}			

			return new ServiceResponse { Succeeded = true };
		}
	}
}
