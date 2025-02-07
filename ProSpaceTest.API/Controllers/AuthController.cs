using Microsoft.AspNetCore.Mvc;
using ProSpaceTest.API.Contracts;
using ProSpaceTest.Core.Abstractions;
using ProSpaceTest.Core.Models;
using System.Threading.Tasks;

namespace ProSpaceTest.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Login) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Login and password are required.");
            }

            try
            {
                var user = await _userService.Authenticate(request.Login, request.Password);

                var response = new AuthResponse(
                    Id: user.Id,
                    Login: user.Login,
                    Role: user.Role,
                    CustomerId: user.CustomerId,
                    Firstname: user.Firstname,
                    Lastname: user.Lastname,
                    IsActive: user.IsActive,
                    Created: user.Created,
                    LastLogin: user.LastLogin
                );

                return Ok(response);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid login or password.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("User ID is required.");
            }

            try
            {
                var user = await _userService.GetUserById(id);

                var response = new UserResponse(
                    Id: user.Id,
                    Login: user.Login,
                    Role: user.Role,
                    CustomerId: user.CustomerId,
                    Firstname: user.Firstname,
                    Lastname: user.Lastname,
                    IsActive: user.IsActive,
                    Created: user.Created,
                    LastLogin: user.LastLogin
                );

                return Ok(response);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("User not found.");
            }
        }
    }
}