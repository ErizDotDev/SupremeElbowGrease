using Microsoft.Extensions.Configuration;
using QLess.Core.Data;
using QLess.Core.Domain;
using QLess.Core.Enums;
using QLess.Core.Interface;
using QLess.DbScriptRunner;
using QLess.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace QLess.Infrastructure.IntegrationTests.Data
{
	public class TransactionRepositoryTests
	{
		private readonly ITransactionRepository _transactionRepository;
		private readonly ICardRepository _cardRepository;
		private readonly Migrations _scriptRunner;

		public TransactionRepositoryTests()
		{
			var config = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.Build();

			_transactionRepository = new TransactionRepository(config);
			_cardRepository = new CardRepository(config);
			_scriptRunner = new Migrations(config);

			ResetDatabase();
		}

		[Fact]
		public async Task GetTripTransactionsForGivenDate_ReturnsCorrectSetOfTransactions_GivenCurrentDateAndValidId()
		{
			ResetDatabase();

			long cardId = CreateCard();

			var transactionsList = new List<Transaction>()
			{
				new Transaction { CardId = cardId, TransactionDate = DateTime.Now, TransactionTypeId = TransactionType.InitialLoad.Id, TransactionAmount = 500m, PreviousBalance = 0m, NewBalance = 500m },
				new Transaction { CardId = cardId, TransactionDate = DateTime.Now, TransactionTypeId = TransactionType.PayTrip.Id, TransactionAmount = 8m, PreviousBalance = 500m, NewBalance = 492m },
				new Transaction { CardId = cardId, TransactionDate = DateTime.Now, TransactionTypeId = TransactionType.PayTrip.Id, TransactionAmount = 7.7m, PreviousBalance = 492m, NewBalance = 484.3m }
			};

			await _transactionRepository.CreateAsync(transactionsList[0]);
			await _transactionRepository.CreateAsync(transactionsList[1]);
			await _transactionRepository.CreateAsync(transactionsList[2]);

			var payTripTransactions = _transactionRepository.GetTripTransactionsForGivenDate(cardId, DateTime.Now);

			Assert.Equal(2, payTripTransactions.Count);
		}

		private void ResetDatabase()
		{
			_scriptRunner.RunReset();
			_scriptRunner.RunScripts();
		}

		private long CreateCard()
		{
			var card = new Card
			{
				CardTypeId = (int)CardType.Regular,
				CardNumber = "221802083101",
				Balance = 100m,
			};

			long cardId = 0;

			_cardRepository.Create(card, out cardId);

			return cardId;
		}
	}
}
