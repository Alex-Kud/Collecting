using Collecting.Data.Models;
using Collecting.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Collecting.Controllers
{    
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        public AuthController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        /// <summary>
        /// Получение токена авторизации
        /// </summary>
        /// <param name="data">Логин и пароль</param>
        /// <returns>Токен авторизации</returns>
        [AllowAnonymous]
        [HttpPost(nameof(Auth))]
        public IActionResult Auth(Login data)
        {
            bool isValid = _userService.IsValidUserInformation(data);
            if (isValid)
            {
                var tokenString = GenerateJwtToken(data.UserEmail);
                return Ok(new { Token = tokenString, Message = "Успех" });
            }
            return BadRequest("Пожалуйста, введите действительное имя пользователя и пароль");
        }

        /// <summary>
        /// Получение результата авторизации
        /// </summary>
        /// <returns>Результат авторизации</returns>
        [Authorize]
        [HttpGet(nameof(GetResult))]
        public IActionResult GetResult() => Ok("Всё ок");

        /// <summary>
        /// Генерация JWT-токена
        /// Чуточка документации по клеймам https://metanit.com/sharp/aspnet5/15.5.php
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        private string GenerateJwtToken(string userEmail)
        {
            var user = _userService.GetUserDetails(userEmail);
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:key"]);

            var claims = new List<Claim> {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userEmail),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
            };

            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
