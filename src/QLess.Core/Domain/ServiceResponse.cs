namespace QLess.Core.Domain
{
	public class ServiceResponse
	{
		public bool Succeeded { get; set; } = false;

		public string Message { get; set; } = string.Empty;
	}
}
