using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Otus.SocNet.DAL;
using Otus.SocNet.WebApi.Models;

namespace Otus.SocNet.WebApi.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, IUserRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            var user = new User
            {
                First_Name = request.First_Name,
                Second_Name = request.Second_Name,
                Biography = request.Biography,
                Birthdate = request.Birthdate,
                City = request.City,                
                Password = HashHalper.Hash(request.Password),                
            };

            var userId = await _repo.CreateAsync(user);
            return Ok(new { 
                user_id = userId,
            });
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return NotFound();

            return Ok(new UserResponse
            {
                Id = user.Id,
                First_Name = user.First_Name,
                Second_Name = user.Second_Name,
                Birthdate = user.Birthdate,
                Biography = user.Biography,
                City = user.City,
            });
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string firstName, [FromQuery] string secondName)
        {
            _logger.LogInformation($"Search: firstName={firstName}, secondName={secondName}");
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(secondName))
            {
                return BadRequest("firstName and secondName are required");
            }

            var users = await _repo.SearchUsers(firstName, secondName);
            return Ok(users);
        }
    }
}
