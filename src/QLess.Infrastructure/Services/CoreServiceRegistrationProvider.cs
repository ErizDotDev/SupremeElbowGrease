using Microsoft.Extensions.DependencyInjection;
using QLess.Core.Interface;

namespace QLess.Infrastructure.Services
{
	public static class CoreServiceRegistrationProvider
	{
		public static void AddCoreServices(this IServiceCollection services)
		{
			services.AddScoped<ICardService, CardService>();
			services.AddScoped<ITripPaymentService, TripPaymentService>();
			services.AddScoped<ICardLoadService, CardLoadService>();
		}
	}
}
