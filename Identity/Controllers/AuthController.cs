using Identity.Models.Request;
using Identity.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(request);

            if (result.Success)
                return Ok(new { message = result.Message });

            return BadRequest(new { message = result.Message });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(request);

            if (result.Success)
            {
                return Ok(new
                {
                    message = result.Message,
                    userId = result.User?.Id,
                    username = result.User?.Username
                });
            }

            return Unauthorized(new { message = result.Message });
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            var user = await _authService.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound("Пользователь не найден");

            return Ok(new
            {
                userId = user.Id,
                username = user.Username,
                email = user.Email,
                isLocked = user.IsLocked,
                lastLoginDate = user.LastLoginDate
            });
        }

        [HttpGet("user/{userId}/roles")]
        public async Task<IActionResult> GetUserRoles(int userId)
        {
            var roles = await _authService.GetUserRolesAsync(userId);
            return Ok(roles);
        }

        [HttpGet("user/{userId}/permissions")]
        public async Task<IActionResult> GetUserPermissions(int userId)
        {
            var permissions = await _authService.GetUserPermissionsAsync(userId);
            return Ok(permissions);
        }
    }
}
