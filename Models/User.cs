using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection.Metadata;

namespace TaskAuthenticationAuthorization.Models
{
    public enum BuyerType
    {
        None, Regular, Golden, Wholesale
    }

    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }
        public int RoleId { get; set; }

        [Required]
        public string Email { get; set; }

        public string Password { get; set; }

        [Display(Name = "Buyer type")]
        public BuyerType TypeOfBuyer { get; set; } = BuyerType.None;

        public Role Role { get; set; }
        public Customer? Customer { get; set; }
    }
}
