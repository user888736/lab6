using System.ComponentModel.DataAnnotations;

namespace WebMvc.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "User password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember you?")]
        public bool RememberLogin { get; set; }

        public string ReturnUrl { get; set; }

    }
}

