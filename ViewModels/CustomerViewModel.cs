using System.ComponentModel.DataAnnotations;
using TaskAuthenticationAuthorization.Models;

namespace TaskAuthenticationAuthorization.ViewModels
{
    public class CustomerViewModel
    {
		[Required(ErrorMessage = "Last name in not specified")]
		[Display (Name ="Last name")]
        public string LastName { get; set; }

        [Display(Name = "First name")]
		[Required(ErrorMessage = "First name in not specified")]
		public string FirstName { get; set; }

        [Required(ErrorMessage = "Email not specified")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password not specified")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password entered incorrectly")]
        public string ConfirmPassword { get; set; }

        public string Address { get; set; }

        [Display(Name = "Buyer type")]
        public BuyerType TypeOfBuyer { get; set; } = BuyerType.None;

        public Discount? Discount { get; set; }
    }
}
