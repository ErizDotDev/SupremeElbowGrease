using Microsoft.Extensions.DependencyInjection;
using QLess.Core.Interface;

namespace QLess.Infrastructure.Data
{
	public static class RepositoryRegistrationProvider
	{
		public static void AddDataRepositories(this IServiceCollection services)
		{
			services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
			services.AddScoped<ICardRepository, CardRepository>();
		}
	}
}
