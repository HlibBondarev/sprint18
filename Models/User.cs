using System.Data;

namespace TaskAuthenticationAuthorization.Models
{
    public enum BuyerType
    {
        None, Regular, Golden, Wholesale
    }
    public class User
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public BuyerType? TypeOfByer { get; set; } = BuyerType.None;

        public int? RoleId { get; set; }
        public Role Role { get; set; }
    }
}
