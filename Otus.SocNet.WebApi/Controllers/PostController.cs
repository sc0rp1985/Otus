using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Otus.SocNet.BLL;
using Otus.SocNet.DAL;
using Otus.SocNet.DAL.Models;
using Otus.SocNet.WebApi.Models;
using System.Security.Claims;

namespace Otus.SocNet.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("post")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IRedisFeedService _feed;

        public PostController(IPostService postService, IRedisFeedService feed)
        {
            _postService = postService;
            _feed = feed;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePost([FromBody] string text)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var post= _postService.CreatePost(userId, text);
            return Ok(new { id = post.Id });
        }

        [HttpGet("feed")]
        public async Task<IActionResult> GetFeed(int offset=0, int limit=10)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var feed = await _feed.GetFeedAsync(userId,offset,limit);
            return Ok(feed);
        }

        [Authorize]
        [HttpPost("feed/rebuild")]
        public async Task<IActionResult> RebuildFeed()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _postService.RebuildFeedForUser(userId);
            return Ok();
        }
    }
}
