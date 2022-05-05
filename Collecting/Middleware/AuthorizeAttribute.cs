using Collecting.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Games.Middleware
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var account = (User) context.HttpContext.Items["User"];
            if (account == null)
            {
                // Не авторизирован
                context.Result = new JsonResult(new { message = "Неавторизован!" }) 
                { 
                    StatusCode = StatusCodes.Status401Unauthorized 
                };
            }
        }
    }
}
