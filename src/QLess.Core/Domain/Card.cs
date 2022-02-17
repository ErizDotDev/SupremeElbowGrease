using QLess.Core.Common;

namespace QLess.Core.Domain
{
	public class Card : BaseEntity
	{
		public int CardType { get; set; }

		public string CardNumber { get; set; }

		public string SpecialIdNumber { get; set; }

		public decimal Balance { get; set; }

		public DateTime? DateLastUsed { get; set; }
	}
}
