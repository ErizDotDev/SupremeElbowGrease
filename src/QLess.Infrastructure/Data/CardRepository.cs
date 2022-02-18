using Dapper;
using Microsoft.Extensions.Configuration;
using QLess.Core.Domain;
using QLess.Core.Interface;

namespace QLess.Infrastructure.Data
{
	public class CardRepository : Repository<Card>, ICardRepository
	{
		public CardRepository(IConfiguration configuration)
			: base(configuration)
		{
		}		

		public Card FindByCardNumber(string cardNumber)
		{
			string sql = "SELECT * FROM CardDetail WHERE CardNumber = @CardNumber";

			dbConnection.Open();

			try
			{
				return dbConnection.Query<Card>(sql, new
				{
					CardNumber = new DbString
					{
						Value = cardNumber,
						IsFixedLength = false,
						IsAnsi = false
					}
				})
				.FirstOrDefault();
			}
			finally
			{
				dbConnection.Close();
			}
		}
	}
}
