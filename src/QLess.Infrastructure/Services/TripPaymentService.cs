using QLess.Core.Data;
using QLess.Core.Domain;
using QLess.Core.Enums;
using QLess.Core.Interface;
using QLess.Core.Services;
using QLess.Infrastructure.Processors;

namespace QLess.Infrastructure.Services
{
	public class TripPaymentService : ITripPaymentService
	{
		private ICardRepository _cardRepository;
		private readonly IRepository<Transaction> _transactionRepository;
		private Dictionary<CardType, Func<BaseCardTransactionProcessor>> cardTransactionProcessorList;

		public TripPaymentService(ICardRepository cardRepository, IRepository<Transaction> transactionRepository)
		{
			_cardRepository = cardRepository;
			_transactionRepository = transactionRepository;
			cardTransactionProcessorList = BaseCardTransactionProcessor.GetAvailableTransactionProcessors();
		}

		public async Task<ServiceResponse> PayForTrip(string cardNumber)
		{
			var cardDetail = _cardRepository.FindByCardNumber(cardNumber);

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
			decimal tripFare = cardTransactionProcessor.Invoke().GetTripFare();

			if (cardDetail.Balance - tripFare <= 0)
			{
				return new ServiceResponse
				{
					Succeeded = false,
					ErrorMessage = "Insufficient load balance. Please reload your card."
				};
			}

			bool isSavePaymentSuccess = await SavePaymentTransaction(cardDetail, tripFare);
			if (!isSavePaymentSuccess)
			{
				return new ServiceResponse
				{
					Succeeded = false,
					ErrorMessage = "Failed to save payment transaction. Please try again."
				};
			}

			return new ServiceResponse { Succeeded = true };
		}

		private async Task<bool> SavePaymentTransaction(Card cardDetail, decimal fare)
		{
			bool result = true;
			decimal newBalance = cardDetail.Balance - fare;

			var paymentTransaction = new Transaction
			{
				CardId = cardDetail.Id,
				TransactionDate = DateTime.Now,
				TransactionTypeId = TransactionType.PayTrip.Id,
				TransactionAmount = fare,
				PreviousBalance = cardDetail.Balance,
				NewBalance = newBalance
			};

			result = result && await _transactionRepository.CreateAsync(paymentTransaction);
			result = result && await SaveNewBalanceInCard(cardDetail, newBalance);

			return result;
		}

		private async Task<bool> SaveNewBalanceInCard(Card cardDetail, decimal newCardBalance)
		{
			cardDetail.Balance = newCardBalance;
			return await _cardRepository.UpdateAsync(cardDetail);
		}
	}
}
