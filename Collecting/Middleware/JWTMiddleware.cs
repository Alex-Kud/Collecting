using Collecting.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Collecting.Middleware
{
    public class JWTMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        //private IUserService _userService;

        public JWTMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
            //_userService = userService;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            //_userService = userService;
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                AttachAccountToContext(context, userService, token);
            }
            
            await _next.Invoke(context);
        }

        private void AttachAccountToContext(HttpContext context, IUserService userService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                string accountEmail = jwtToken.Claims
                    .First(x => x.Type == ClaimsIdentity.DefaultNameClaimType)
                    .Value;

                // Прикрепить учетную запись к контексту при успешной проверке jwt
                var user = userService.GetUserDetails(accountEmail);
                if (user != null)
                {
                    context.Items["User"] = user;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {

                // Ничего не делать, если проверка jwt завершается неудачей
                // Учетная запись не привязана к контексту, поэтому запрос не будет иметь доступа к защищенным маршрутам
            }
        }
    }
}
