namespace QLess.Core.Domain
{
	public class TripPaymentDetail
	{
		public decimal Fare { get; set; } = 0m;

		public decimal PreviousCardBalance { get; set; } = 0m;

		public decimal NewCardBalance 
		{
			get { return PreviousCardBalance - Fare; }
		}
	}

	public class TripPaymentResponse : ServiceResponse
	{
		public TripPaymentDetail TransactionDetail { get; set; }
	}
}
