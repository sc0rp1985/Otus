using Microsoft.Extensions.DependencyInjection;

namespace Otus.SocNet.DAL
{
    public static class ContainerConfig
    {
        public static IServiceCollection AddDal(this IServiceCollection services, string connectionString)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            services.AddScoped<IUserRepository, UserRepository>(_=> new(connectionString));
            services.AddSingleton<IDbInitializer,DbInitializer>();            
            services.AddScoped<ISocialRepository,SocialRepository>(_=> new(connectionString));
            return services;
        }

    }
}
