using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Otus.SocNet.DAL;
using Otus.SocNet.WebApi.Models;

namespace Otus.SocNet.WebApi.Controllers
{

    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _repo;

        public AuthController(IUserRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!Int32.TryParse(request.Id,out int id))
                return BadRequest();
            var user = await _repo.GetByIdAsync(id);
            if (user == null || !HashHalper.Verify(request.Password, user.Password))
            {
                return Unauthorized();
            }

            return Ok(new { token = "mock-jwt-token" });
        }
    }
}
