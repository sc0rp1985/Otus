using Microsoft.Extensions.DependencyInjection;

namespace Otus.SocNet.DAL
{
    public static class ContainerConfig
    {
        public static IServiceCollection AddDal(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<IUserRepository, UserRepository>(_=> new(connectionString));
            services.AddSingleton<IDbInitializer,DbInitializer>();            
            return services;
        }

    }
}
