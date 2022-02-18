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

		public List<Transaction> GetTripTransactionsForGivenDate(DateTime targetDate)
		{
			string sql = "SELECT * FROM [Transaction] WHERE [TransactionTypeId] = @TransactionTypeId " +
				"AND [TransactionDate] >= @StartDate AND [TransactionDate] <= @EndDate";

			dbConnection.Open();

			try
			{
				var sqlParams = new DynamicParameters();
				sqlParams.Add("@TransactionTypeId", TransactionType.PayTrip.Id);
				sqlParams.Add("@StartDate", targetDate.GetDayStartDateTime());
				sqlParams.Add("@EndDate", targetDate.GetDayEndDateTime());

				return dbConnection.Query<Transaction>(sql, sqlParams).ToList();
			}
			finally
			{
				dbConnection.Close();
			}
		}
	}
}
