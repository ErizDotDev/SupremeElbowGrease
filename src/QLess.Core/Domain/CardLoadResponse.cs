namespace QLess.Core.Domain
{
	public class LoadPaymentDetail
	{
		public decimal LoadAmount { get; set; } = 0m;

		public decimal AmountPaid { get; set; } = 0m;

		public decimal Change { get; set; } = 0m;

		public decimal CardBalance { get; set; } = 0m;
	}

	public class CardLoadResponse
	{
		public LoadPaymentDetail PaymentDetail { get; set; }

		public bool Succeeded { get; set; }

		public string Message { get; set; }
	}
}
