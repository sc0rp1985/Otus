using Otus.SocNet.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otus.SocNet.DAL
{
    public  interface ISocialRepository
    {
        Task AddFriendAsync(int userId, int friendId);
        Task DeleteFriendAsync(int userId, int friendId);
        Task<int> CreatePostAsync(Post post);
        Task<List<int>> GetFollowersAsync(int authorId);
        Task<List<Post>> GetFeedAsync(int userId);        
    }
}
