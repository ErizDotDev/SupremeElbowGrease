using QLess.Core.Domain;
using QLess.Core.Enums;
using QLess.Core.Interface;
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

		public async Task<TripPaymentResponse> PayForTrip(string cardNumber)
		{
			var cardDetail = await _cardService.FindCardDetailsByCardNumber(cardNumber);

			if (cardDetail == null)
			{
				return new TripPaymentResponse
				{ 
					Succeeded = false,
					Message = "Card number is not found."
				};
			}

			if (cardDetail.Balance <= 0)
			{
				return new TripPaymentResponse
				{
					Succeeded = false,
					Message = "Insufficient load balance. Please reload your card."
				};
			}			

			var cardTransactionProcessor = cardTransactionProcessorList[(CardType)cardDetail.CardTypeId];

			bool isCardExpired = cardTransactionProcessor.Invoke().IsCardExpired(cardDetail.DateLastUsed.Value, DateTime.Now);
			if (isCardExpired)
			{
				return new TripPaymentResponse
				{
					Succeeded = false,
					Message = "Card is already expired. Please create another card."
				};
			}

			var currentDateTripTransactions = await _transactionService.GetTripTransactionsFromGivenDate(cardDetail.Id, DateTime.Now);
			decimal tripFare = cardTransactionProcessor.Invoke().GetTripFare(currentDateTripTransactions);
			decimal newCardBalance = cardDetail.Balance - tripFare;

			if (cardDetail.Balance < tripFare)
			{
				return new TripPaymentResponse
				{
					Succeeded = false,
					Message = "Insufficient load balance. Please reload your card."
				};
			}

			var transactionDetail = new TripPaymentDetail
			{ 
				Fare = tripFare,
				PreviousCardBalance = cardDetail.Balance
			};

			bool isSaveCardBalanceSuccess = await _cardService.SaveNewCardBalance(cardDetail, newCardBalance);
			if (!isSaveCardBalanceSuccess)
			{
				return new TripPaymentResponse
				{
					Succeeded = false,
					Message = "Failed to save new card balance. Please try again."
				};
			}

			bool isSavePaymentTransactionSuccess = await _transactionService.SaveTripPaymentTransaction(cardDetail, tripFare);
			if (!isSavePaymentTransactionSuccess)
			{
				return new TripPaymentResponse
				{
					Succeeded = false,
					Message = "Failed to save payment transaction."
				};
			}			

			return new TripPaymentResponse 
			{ 
				Succeeded = true,
				TransactionDetail = transactionDetail
			};
		}
	}
}
