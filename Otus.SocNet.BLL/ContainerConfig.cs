using Microsoft.Extensions.DependencyInjection;
namespace Otus.SocNet.BLL
{
    public static class ContainerConfig
    {
        public static IServiceCollection AddBll(this IServiceCollection services)
        {            
            services.AddScoped<IRedisFeedService, RedisFeedService>();
            services.AddScoped<IPostService, PostService>();
            return services;
        }

    }
}
