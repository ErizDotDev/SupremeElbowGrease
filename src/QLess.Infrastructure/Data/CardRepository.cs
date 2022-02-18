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

		public bool CreateCard(Card entity, out long id)
		{
			return CreateCard(entity, out id);
		}

		public async Task<bool> CreateCardAsync(Card entity)
		{
			return await CreateCardAsync(entity);
		}

		public async Task<bool> DeleteCardAsync(long id)
		{
			return await DeleteCardAsync(id);
		}

		public async Task<IQueryable<Card>> FindAllCardsAsync()
		{
			return await FindAllAsync();
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

		public async Task<Card> FindCardByIdAsync(long id)
		{
			return await FindCardByIdAsync(id);
		}

		public async Task<bool> UpdateCardAsync(Card entity)
		{
			return await UpdateAsync(entity);
		}
	}
}
