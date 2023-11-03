using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VideoCall.EF.Models;
using VideoCall.Identity.Models;
using VideoCall.RepoModel.Repos;

namespace VideoCall.Pages
{
    [Authorize]
    public class LobbyModel : PageModel
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly RoomRepo _roomRepo;
        public LobbyModel(RoomRepo roomRepo,UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _roomRepo = roomRepo;
        }
        public void OnGet()
        {
        }
        public IActionResult OnPostAsync() {
            ApplicationUser user = _userManager.FindByNameAsync(HttpContext.User.Identity!.Name).Result;
            var room = _roomRepo.GetAsync(Request.Form["roomid"]).Result;
            DateTime begintime = DateTime.Parse(room!.BeginDateTime);
            DateTime endtime = DateTime.Parse(room!.EndDateTime);

            if (room !=null&&DateTime.Now.Ticks > begintime.Ticks && endtime.Ticks > DateTime.Now.Ticks) {
                if (!room.CallHistory.Any(x=>x.User.UserName==user.UserName))
                {
                    room.CallHistory.Add(new Call { User=user,StartDate=DateTime.Now });
                }
                else
                {
                    room.CallHistory.First(x => x.User.UserName == user.UserName).StartDate = DateTime.Now;
                }
                _roomRepo.UpdateAsync(room.RoomId.ToString(), room);
                HttpContext.Session.SetString("roomenddatetime",endtime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
                HttpContext.Session.SetString("username", HttpContext!.User.Identity!.Name);
                return RedirectToPage("/ChatRoom", new { room = Request.Form["roomid"] });
            }
            TempData["RoomError"] = "Error";
            return Page();

        }
    }
}
