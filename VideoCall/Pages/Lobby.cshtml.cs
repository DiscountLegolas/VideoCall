using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VideoCall.Pages
{
    public class LobbyModel : PageModel
    {
        public void OnGet()
        {
        }
        public IActionResult OnPostAsync() {
            Console.WriteLine(Request.Form["username"]);
            HttpContext.Session.SetString("username", Request.Form["username"]);
            return RedirectToPage("/ChatRoom", new { room = Request.Form["roomid"] });
        }
    }
}
