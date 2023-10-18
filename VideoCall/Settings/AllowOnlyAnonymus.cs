using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace VideoCall.Settings
{
    public class AllowOnlyAnonymus: IAuthorizationFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AllowOnlyAnonymus(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (_httpContextAccessor.HttpContext!.User.Identity!.IsAuthenticated)
            {
                context.Result=new RedirectToPageResult("")
            }
        }
    }
}
