using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProSpaceTest.API.Contracts;
using ProSpaceTest.Application.Services;
using ProSpaceTest.Core.Abstractions;
using ProSpaceTest.Core.Models;
using ProSpaceTest.DataAccess.Repository;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ProSpaceTest.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAccountCreationService _accountCreationService;
        private readonly IUserService _userService;

        public UserController(IAccountCreationService accountCreationRepository, IUserService userService)
        {
            _accountCreationService = accountCreationRepository;
            _userService = userService;
        }

        [HttpPost("CreateAccount")]
        public async Task<IActionResult> Create([FromBody] CreateAccountRequest request)
        {


            var (user, errorUser) = Core.Models.User.Create(
                Guid.NewGuid(),
                request.Login,
                request.Password,
                request.Role,
                true);

            if (!string.IsNullOrEmpty(errorUser))
            {
                return BadRequest(errorUser);
            }
            var (customer, errorCustomer) = Customer.Create(
                Guid.NewGuid(),
                user.Id,
                request.Name,
                request.Code,
                request.Address,
                request.Discount);

            if (!string.IsNullOrEmpty(errorCustomer))
            {
                return BadRequest(errorCustomer);
            }

            try
            {
                var id = await _accountCreationService.CreateAccount(customer, user);
                return CreatedAtAction(nameof(GetUserById), new { id }, id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

                if (user.IsActive == true)
                {
                    var response = new AuthResponse(
                        Id: user.Id,
                        Login: user.Login,
                        Role: user.Role,
                        IsActive: user.IsActive
                    );

                    return Ok(response);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid login or password.");
            }
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _userService.DeleteUser(id);
                return Accepted();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("User not found");
            }
        }
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            var response = users.Select(u => new UserResponse(u.Id, u.Login, u.Role, u.IsActive));
            return Ok(response);
        }
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = new Core.Models.User
                {
                    Id = id,
                    Login = request.Login,
                    Password = request.Password,
                    IsActive = request.IsActive
                };

                var updatedId = await _userService.UpdateUser(user);
                return Accepted();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
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
                    IsActive: user.IsActive
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