using Microsoft.Extensions.DependencyInjection;
using QLess.Web.Interfaces;

namespace QLess.Web.Services
{
	public static class ClientServiceRegistrationProvider
	{
		public static IServiceCollection AddClientServices(this IServiceCollection services)
		{
			return services.AddScoped<IQLessClientService, CardClientService>();
		}
	}
}
