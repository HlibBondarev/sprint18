using System.ComponentModel.DataAnnotations;
using TaskAuthenticationAuthorization.Models;

namespace TaskAuthenticationAuthorization.ViewModels
{
    public class CustomerViewModel
    {

        [Display (Name ="Last name")]
        public string LastName { get; set; }
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Email not specified")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password not specified")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password entered incorrectly")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Buyer type")]
        public BuyerType TypeOfBuyer { get; set; } = BuyerType.None;

        public string Address { get; set; }
        public Discount? Discount { get; set; }
    }
}
