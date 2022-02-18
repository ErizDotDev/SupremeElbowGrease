using QLess.Core.Common;

namespace QLess.Core.Domain
{
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
