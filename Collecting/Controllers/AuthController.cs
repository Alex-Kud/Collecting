using Collecting.Data.Models;
using Collecting.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Games.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        public AuthController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

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

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
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
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:key"]);
            var user = _userService.GetUserDetails(userEmail);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, userEmail),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
