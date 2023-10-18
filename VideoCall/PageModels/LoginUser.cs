using System.ComponentModel.DataAnnotations;

namespace VideoCall.PageModels
{
    public class LoginUser
    {
        [Required]
        public string UserNameOrEmail { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
