using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VideoCall.EF.Models;
using VideoCall.PageModels;
using VideoCall.RepoModel.Repos;

namespace VideoCall.Pages
{
    public class ChatRoomModel : PageModel
    {
        private readonly ILogger<ChatRoomModel> _logger;
        public ChatRoomModel(ILogger<ChatRoomModel> logger)
        {
            _logger = logger;
        }
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("username")!=null)
            {
                return Page();
            }
            TempData["RoomError"] = "Error";
            return RedirectToPage("/Lobby");
        }
    }
}