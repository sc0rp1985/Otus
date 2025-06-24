using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Otus.SocNet.DAL;
using Otus.SocNet.WebApi.Models;
using System.Security.Claims;

namespace Otus.SocNet.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("friend")]
    public class FriendController : ControllerBase
    {
        private readonly ISocialRepository _repo;

        public FriendController(ISocialRepository repo)
        {
            _repo = repo;
        }


        [HttpPost("set/{user_id}")]
        public async Task<IActionResult> AddFriend(int user_id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _repo.AddFriendAsync(userId, user_id);
            return Ok();
        }

        [HttpPost("delete/{user_id}")]
        public async Task<IActionResult> DeleteFriend( int user_id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _repo.DeleteFriendAsync(userId, user_id);
            return Ok();
        }
    }
}
