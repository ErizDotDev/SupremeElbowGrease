using QLess.Core.Enums;

namespace QLess.Web.Models
{
	public class CreateCardRequest
	{
		public CardType CardTypeEnum { get; set; } = Core.Enums.CardType.Regular;

		public int CardType { get { return (int)CardTypeEnum; } }

		public decimal InitialLoadAmount { get; set; } = 100m;

		public string SpecialIDNumber { get; set; } = string.Empty;
	}
}
