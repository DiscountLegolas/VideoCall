using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using VideoCall.Identity.Models;
using VideoCall.PageModels;

namespace VideoCall.Pages
{
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public class LoginModel : PageModel
    {
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;
        public LoginModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public void OnGet()
        {
        }
        [BindProperty]
        public LoginUser? user { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                ApplicationUser appUseremail = await userManager.FindByEmailAsync(user.UserNameOrEmail);
                ApplicationUser appUsername = await userManager.FindByNameAsync(user.UserNameOrEmail);
                if (appUseremail != null)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(appUseremail, user.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToPage();
                    }
                }
                if (appUsername!=null)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(appUsername, user.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToPage();
                    }
                }
                ModelState.AddModelError(nameof(user.UserNameOrEmail), "Login Failed: Invalid Email or Password");
            }

            return Page();
        }
    }
}
