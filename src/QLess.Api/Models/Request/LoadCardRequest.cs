namespace QLess.Api.Models.Request
{
	public class LoadCardRequest
	{
		public string CardNumber { get; set; }

		public decimal LoadAmount { get; set; }

		public decimal AmountPaid { get; set; }
	}
}
