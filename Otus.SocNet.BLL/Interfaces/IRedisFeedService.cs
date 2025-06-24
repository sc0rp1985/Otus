using Otus.SocNet.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otus.SocNet.BLL
{
    public interface IRedisFeedService
    {
        Task AddPostToFeedsAsync(Post content, List<int> followers);
        Task<List<string>> GetFeedAsync(int userId,int offset = 0, int limit = 10);
        Task RebuildFeedAsync(int userId, Func<int, Task<List<Post>>> fetchFromDb);
    }
}
