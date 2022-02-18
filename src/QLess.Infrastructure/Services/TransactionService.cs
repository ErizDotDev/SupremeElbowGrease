﻿using QLess.Core.Data;
using QLess.Core.Domain;
using QLess.Core.Interface;

namespace QLess.Infrastructure.Services
{
	public class TransactionService : ITransactionService
	{
		private readonly IRepository<Transaction> _transactionRepository;

		public TransactionService(IRepository<Transaction> transactionRepository)
		{
			_transactionRepository = transactionRepository;
		}

		public async Task<bool> SaveCreateCardTransaction(Card cardDetail)
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

		public async Task<bool> SaveTripPaymentTransaction(Card cardDetail, decimal fare)
		{
			var paymentTransaction = new Transaction
			{
				CardId = cardDetail.Id,
				TransactionDate = DateTime.Now,
				TransactionTypeId = TransactionType.PayTrip.Id,
				TransactionAmount = fare,
				PreviousBalance = cardDetail.Balance,
				NewBalance = cardDetail.Balance - fare
			};

			return await _transactionRepository.CreateAsync(paymentTransaction);
		}
	}
}
