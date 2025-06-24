using Otus.SocNet.DAL;
using Otus.SocNet.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otus.SocNet.BLL
{
    public class PostService : IPostService
    {
        private readonly ISocialRepository _repo;
        private readonly IRedisFeedService _feed;
        public PostService(ISocialRepository repo, IRedisFeedService feed) 
        {
            _repo = repo;
            _feed = feed;
        }

        public async Task<Post> CreatePost(int autorid, string text)
        {
            var post = new Post
            {
                AuthorId = autorid,
                Content = text,
                CreatedAt = DateTime.Now.ToUniversalTime(),
            };
            var postId = await _repo.CreatePostAsync(post);
            post.Id = postId;
            var followers = await _repo.GetFollowersAsync(autorid);
            await _feed.AddPostToFeedsAsync(post, followers);            
            return post;
        }

        public async Task RebuildFeedForUser(int userId)
        {
            var posts = await _repo.GetFeedAsync(userId);
            await _feed.RebuildFeedAsync(userId, _ => Task.FromResult(posts));
        }
    }
}
