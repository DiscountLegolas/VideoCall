using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace VideoCall.Settings
{
    public class AllowOnlyAnonymus: Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext!.User.Identity!.IsAuthenticated)
            {
                Console.WriteLine("aadad");
                context.Result = new RedirectToPageResult("LandingLogged");
            }
        }

    }
}
