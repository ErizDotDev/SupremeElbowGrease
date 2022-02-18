using QLess.Core.Domain;

namespace QLess.Core.Interface
{
	public interface ICardRepository : IRepository<Card>
	{		
		Card FindByCardNumber(string cardNumber);
	}
}
