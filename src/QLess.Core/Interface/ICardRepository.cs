using QLess.Core.Domain;

namespace QLess.Core.Interface
{
	public interface ICardRepository : IRepository<Card>
	{
		Task<bool> CreateCardAsync(Card entity);
		bool CreateCard(Card entity, out long id);
		Task<bool> DeleteCardAsync(long id);
		Task<IQueryable<Card>> FindAllCardsAsync();
		Card FindByCardNumber(string cardNumber);
		Task<Card> FindCardByIdAsync(long id);
		Task<bool> UpdateCardAsync(Card entity);
	}
}
