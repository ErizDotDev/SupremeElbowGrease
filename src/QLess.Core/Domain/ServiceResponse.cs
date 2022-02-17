namespace QLess.Core.Services
{
	public class ServiceResponse
	{
		public bool Succeeded { get; set; } = false;

		public string ErrorMessage { get; set; } = string.Empty;
	}
}
