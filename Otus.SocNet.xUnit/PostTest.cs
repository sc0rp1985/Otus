using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Otus.SocNet.BLL;
using Otus.SocNet.DAL;
using Otus.SocNet.DAL.Models;
using StackExchange.Redis;
using Xunit;

namespace Otus.SocNet.xUnit
{
    public class PostTest
    {
        readonly ServiceProvider _serviceProvider;
        public PostTest()
        {
            var serviceCollection = new ServiceCollection();

            var configuration = new ConfigurationBuilder().
                AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).
                AddEnvironmentVariables().
                Build();


            serviceCollection.AddBll();
            var cs =
#if !DEBUG
            configuration.GetConnectionString("DBConnection");
#else
            configuration.GetConnectionString("DBConnectionDebug");
#endif
            serviceCollection.AddDal(cs);
            var rc =
#if !DEBUG
    configuration.GetConnectionString("RedisConnection");
#else
    configuration.GetConnectionString("RedisConnectionDebug");
#endif


            serviceCollection.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(rc));

            _serviceProvider = serviceCollection.BuildServiceProvider();


        }


        [Fact]
        public async Task GenPostTest()
        {
            var srv = _serviceProvider.GetRequiredService<ISocialRepository>();
            var postSrv = _serviceProvider.GetService<IPostService>();
            var users = new List<int> { 999933, 999934 };
            for (int i = 0; i <= 1100; i++)
            {
                foreach (var user in users)
                {
                    var text = $"post {i} by user {user}";
                    await postSrv.CreatePost(user,text);
                }
            }
        }


    }
}
