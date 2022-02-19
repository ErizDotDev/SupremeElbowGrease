using QLess.Core.Domain;

namespace QLess.Core.Interface
{
	public interface ICardLoadService
	{
		Task<CardLoadResponse> LoadCard(string cardNumber, decimal loadAmount, decimal amountPaid);
	}
}
