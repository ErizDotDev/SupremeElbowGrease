using Dapper;
using Microsoft.Extensions.Configuration;
using QLess.Core.Data;
using QLess.Core.Domain;
using QLess.Core.Helpers;
using QLess.Core.Interface;

namespace QLess.Infrastructure.Data
{
	public class TransactionRepository : Repository<Transaction>, ITransactionRepository
	{
		public TransactionRepository(IConfiguration configuration) : base(configuration)
		{
		}

		public List<Transaction> GetTripTransactionsForGivenDate(long cardId, DateTime targetDate)
		{
			string sql = "SELECT * FROM [CardTransaction] WHERE [TransactionTypeId] = @TransactionTypeId " +
				"AND [TransactionDate] >= @StartDate AND [TransactionDate] <= @EndDate AND" +
				"[CardId] = @CardId";

			dbConnection.Open();

			try
			{
				var sqlParams = new DynamicParameters();
				sqlParams.Add("@TransactionTypeId", TransactionType.PayTrip.Id);
				sqlParams.Add("@StartDate", targetDate.GetDayStartDateTime());
				sqlParams.Add("@EndDate", targetDate.GetDayEndDateTime());
				sqlParams.Add("@CardId", cardId);

				return dbConnection.Query<Transaction>(sql, sqlParams).ToList();
			}
			finally
			{
				dbConnection.Close();
			}
		}

		public List<Transaction> GetCardTransactions(string cardNumber)
		{
			string sql = "SELECT " +
				"T.* " +
				"FROM[CardTransaction] T " +
				"INNER JOIN[CardDetail] C ON C.[Id] = T.[CardId] " +
				"WHERE C.[CardNumber] = @CardNumber";

			dbConnection.Open();

			try
			{
				var sqlParams = new DynamicParameters();
				sqlParams.Add("@CardNumber", cardNumber);

				return dbConnection.Query<Transaction>(sql, sqlParams).ToList();
			}
			finally
			{
				dbConnection.Close();
			}
		}
	}
}
