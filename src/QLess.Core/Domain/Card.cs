using QLess.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLess.Core.Domain
{
	[Table("CardDetail")]
	public class Card : BaseEntity
	{
		public int CardTypeId { get; set; }

		public string CardNumber { get; set; }

		public string SpecialIdNumber { get; set; }

		public decimal Balance { get; set; }

		public DateTime? DateLastUsed { get; set; }
	}
}
