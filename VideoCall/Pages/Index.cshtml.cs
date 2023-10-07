using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VideoCall.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private Microsoft.Extensions.Hosting.IHostingEnvironment _environment;

        public IndexModel(ILogger<IndexModel> logger, Microsoft.Extensions.Hosting.IHostingEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }
        [BindProperty]
        public IFormFile Upload { get; set; }
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