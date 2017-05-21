using System.ComponentModel.DataAnnotations;

namespace DBModel.Models.Identity
{
    public class LoginViewModel
    {
        [Display(Name = "Username")]
        [Required]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
    }
}