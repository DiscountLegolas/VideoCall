using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VideoCall.Identity.Models;
using VideoCall.PageModels;

namespace VideoCall.Pages
{
    public class SignUpModel : PageModel
    {
        private UserManager<ApplicationUser> userManager;
        public SignUpModel(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public void OnGet()
        {
        }
        [BindProperty]
        public User? user { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                ApplicationUser appUser = new ApplicationUser
                {
                    UserName = user.UserName,
                    Email = user.Email
                };

                IdentityResult result = await userManager.CreateAsync(appUser, user.Password);
                if (result.Succeeded)
                {
                    Console.WriteLine("noerror");
                    return RedirectToPage("/SignUpSuccessful");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        Console.WriteLine(error.Description);
                        ModelState.AddModelError("", error.Description);
                    }
                    return Page();
                }
            }
            else
            {
                return Page();
            }
        }
    }
}
