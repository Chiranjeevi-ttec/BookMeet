using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;


namespace BookMeet.API.Extensions
{
    /// <summary>
    /// AuthorizeAttribute
    /// </summary>
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// AuthorizeAttribute Constructor
        /// </summary>
        /// <param name="scope"></param>
        public AuthorizeAttribute() : base(typeof(AuthorizeFilter))
        {
           // Arguments = new object[] { scope };
        }
    }
    /// <summary>
    /// Authorization filter 
    /// </summary>
    public class AuthorizeFilter : IAuthorizationFilter
    {
        
        private readonly IConfiguration _config;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public AuthorizeFilter(IConfiguration config)
        {
            _config = config;
        }
        /// <summary>
        /// This will Authorize User 
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var IsAuthenticated = context.HttpContext.User.Identity.IsAuthenticated;
            if (IsAuthenticated!=true)
            {
                var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var handler = new JwtSecurityTokenHandler();
                if (string.IsNullOrEmpty(token))
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Result = new JsonResult("NotAuthorized")
                    {
                        Value = new
                        {
                            Status = "Error",
                            Message = new UnauthorizedAccessException("Unauthenticated User: Token can not be null")
                        },
                    };
                }

            }
            
            return;
        }
    }
}
