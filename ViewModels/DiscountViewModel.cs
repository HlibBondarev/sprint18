using System.ComponentModel.DataAnnotations;
using TaskAuthenticationAuthorization.Models;

namespace TaskAuthenticationAuthorization.ViewModels
{
    public class DiscountViewModel
    {
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        [Display(Name = "Buyer type")]
        public string TypeOfBuyer { get; set; }
        public string Discount { get; set; }
    }
}
