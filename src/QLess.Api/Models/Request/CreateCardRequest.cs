namespace QLess.Api.Models.Request
{
	public class CreateCardRequest
	{
		public int CardType { get; set; }

		public decimal InitialLoadAmount { get; set; }

		public string SpecialIDNumber { get; set; } = string.Empty;
	}
}
