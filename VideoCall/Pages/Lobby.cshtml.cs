using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VideoCall.Pages
{
    public class LobbyModel : PageModel
    {
        public void OnGet()
        {
        }
        public void OnPost()
        {
            Console.WriteLine(Request.Form["roomid"]);
        }
    }
}
