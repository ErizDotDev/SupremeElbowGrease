namespace QLess.Web.Models
{
	public class CardLoadRequest
	{
		public string CardNumber { get; set; } = String.Empty;

		public decimal LoadAmount { get; set; } = 0m;

		public decimal AmountPaid { get; set; } = 0m;
	}
}
