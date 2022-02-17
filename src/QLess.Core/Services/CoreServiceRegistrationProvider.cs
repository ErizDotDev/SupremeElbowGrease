using Microsoft.Extensions.DependencyInjection;
using QLess.Core.Interface;

namespace QLess.Core.Services
{
	public static class CoreServiceRegistrationProvider
	{
		public static void AddCoreServices(this IServiceCollection services)
		{
			services.AddScoped<ICardService, CardService>();
		}
	}
}
