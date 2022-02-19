using QLess.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLess.Core.Domain
{
	[Table("CardTransaction")]
	public class Transaction : BaseEntity
	{
		public long CardId { get; set; }

		public DateTime TransactionDate { get; set; }

		public int TransactionTypeId { get; set; }

		public decimal TransactionAmount { get; set; }

		public decimal PreviousBalance { get; set; }

		public decimal NewBalance { get; set; }
	}
}
