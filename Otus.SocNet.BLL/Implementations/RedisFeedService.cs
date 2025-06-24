using Otus.SocNet.DAL.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Otus.SocNet.BLL
{
    public class RedisFeedService : IRedisFeedService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public RedisFeedService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = _redis.GetDatabase();
        }

        public async Task AddPostToFeedsAsync(Post post, List<int> followers)
        {
            var postJson = JsonSerializer.Serialize(post);

            foreach (var userId in followers)
            {
                var key = $"feed:{userId}";
                await _db.ListLeftPushAsync(key, postJson);
                await _db.ListTrimAsync(key, 0, 999);
            }
        }

        public async Task<List<string>> GetFeedAsync(int userId, int offset = 0, int limit = 10)
        {
            var key = $"feed:{userId}";
            var items = await _db.ListRangeAsync(key, offset, offset+limit);
            return items.Select(x => x.ToString()).ToList();
        }

        public async Task RebuildFeedAsync(int userId, Func<int, Task<List<Post>>> fetchFromDb)
        {
            var key = $"feed:{userId}";

            // Очистить текущий кэш
            await _db.KeyDeleteAsync(key);

            // Получить из БД
            var posts = await fetchFromDb(userId);
            foreach (var post in posts)
            {
                var postJson = JsonSerializer.Serialize(post);

                await _db.ListRightPushAsync(key, postJson);
            }

            await _db.ListTrimAsync(key, 0, 999); // для надёжности
        }
    }
}
