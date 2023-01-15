using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System.Net;
using Whatsapp_bot.ServiceContracts;

namespace Whatsapp_bot.Utils.Middleware
{
    public class EmeraldAuthorizationAttribute : TypeFilterAttribute
    {
        public EmeraldAuthorizationAttribute() : base(typeof(EmeraldAuthorizationFilter))
        {

        }
    }
    public class EmeraldAuthorizationFilter : IAuthorizationFilter
    {
        readonly IUserInformationService _userInformationService;
        public EmeraldAuthorizationFilter(
            IUserInformationService userInformationService)
        {
            _userInformationService = userInformationService;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Request.Headers.TryGetValue("X-User-Key", out StringValues value))
            {
                string userId = value;
                if (Guid.TryParse(userId, out Guid userIdGuid))
                {
                    var user = _userInformationService.GetUserAsync(userIdGuid).GetAwaiter().GetResult();
                    if (user != null)
                        return;
                }
            }
            context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
        }
    }
}
