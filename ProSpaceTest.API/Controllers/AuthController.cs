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

        /// <summary>
        /// Аутентификация пользователя.
        /// </summary>
        /// <param name="request">Запрос с логином и паролем</param>
        /// <returns>Информация о пользователе или ошибка 401 (Unauthorized)</returns>
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthRequest request)
        {
            // Проверяем входные данные
            if (string.IsNullOrWhiteSpace(request.Login) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Login and password are required.");
            }

            try
            {
                // Вызываем сервис для аутентификации
                var user = await _userService.Authenticate(request.Login, request.Password);

                // Преобразуем модель User в AuthResponse
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

        /// <summary>
        /// Получение пользователя по ID.
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <returns>Информация о пользователе или ошибка 404 (Not Found)</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            // Проверяем входные данные
            if (id == Guid.Empty)
            {
                return BadRequest("User ID is required.");
            }

            try
            {
                // Вызываем сервис для получения пользователя
                var user = await _userService.GetUserById(id);

                // Преобразуем модель User в UserResponse
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