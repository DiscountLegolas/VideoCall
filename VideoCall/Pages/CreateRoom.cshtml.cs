using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VideoCall.EF.Models;
using VideoCall.Identity.Models;
using VideoCall.PageModels;
using VideoCall.RepoModel.Repos;

namespace VideoCall.Pages
{
    [Authorize]
    public class CreateRoomModel : PageModel
    {
        private readonly RoomRepo _roomRepo;
        private UserManager<ApplicationUser> _userManager;
        public CreateRoomModel(RoomRepo roomRepo,UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _roomRepo = roomRepo;
        }
        public void OnGet()
        {
            
        }
        public IActionResult OnPostCreateRoom([FromBody] RoomCreate roomcreate) {
            var topic = roomcreate.Topic;
            var time =Convert.ToDouble(roomcreate.Time);
            var begindate = DateTime.ParseExact(roomcreate.Begindate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
            Console.WriteLine(begindate.ToString());
            Room room = new Room { BeginDateTime=begindate.ToString(),EndDateTime=begindate.AddHours(time).ToString(),Topic=topic};
            room.CreatedAt = DateTime.Now.ToString();
            room.CreatorUser=_userManager.FindByNameAsync(HttpContext.User.Identity!.Name).Result;
            _roomRepo.CreateAsync(room);
            return new JsonResult("Successfully Created");
        }
    }
}
