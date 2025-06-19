using Microsoft.AspNetCore.Mvc;
using ReadMangaWS.Models;
using ReadMangaWS.Repository;

namespace ReadMangaWS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // POST /Auth/login
        // Тело запроса: { "username": "user", "password": "pass" }
        [HttpPost("login")]
        public ActionResult<User> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
                return BadRequest("Не указан логин или пароль");

            // Здесь нужно захешировать пароль так же, как в базе (например, SHA256)
            string passwordHash = HashPassword(loginRequest.Password);

            var users = _userRepository.AuthorizationUser(loginRequest.Username, passwordHash);

            if (users.Count == 0)
                return Unauthorized("Неверный логин или пароль");

            var user = users[0];

            // Устанавливаем текущего пользователя в синглтон сессии
            UserSession.Instance.CurrentUser = user;

            return Ok(user);
        }

        string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] hash = sha256.ComputeHash(bytes);
            return Convert.ToHexString(hash).ToLower();
        }

        public class LoginRequest
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
    }
}
