using QLess.Core.Domain;
using QLess.Core.Enums;
using QLess.Web.Models;

namespace QLess.Web.Interfaces
{
	public interface ICardClientService
	{
		Task<CreateCardResponse> CreateCard(CreateCardRequest createCardRequest);
	}
}
