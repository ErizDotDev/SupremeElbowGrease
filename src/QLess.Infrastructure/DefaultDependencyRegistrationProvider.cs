using Microsoft.Extensions.DependencyInjection;
using QLess.Infrastructure.Data;
using QLess.Infrastructure.Services;

namespace QLess.Infrastructure
{
	public static class DefaultDependencyRegistrationProvider
	{
		public static void AddQLess(this IServiceCollection services)
		{
			services.AddDataRepositories();
			services.AddCoreServices();
		}
	}
}
