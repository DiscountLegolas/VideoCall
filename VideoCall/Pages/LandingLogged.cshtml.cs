using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VideoCall.Pages
{
    [Authorize]
    public class LandingLoggedModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
