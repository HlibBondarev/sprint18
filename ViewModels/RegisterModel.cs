using System.ComponentModel.DataAnnotations;
using TaskAuthenticationAuthorization.Models;

namespace TaskAuthenticationAuthorization.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Email not specified")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password not specified")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password entered incorrectly")]
        public string ConfirmPassword { get; set; }

        [Display(Name ="Buyer type")]
        public BuyerType TypeOfByer { get; set; } = BuyerType.Regular;
    }
}
