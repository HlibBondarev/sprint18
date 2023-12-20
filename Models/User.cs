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
		public string Email { get; set; }
		public string Password { get; set; }
		public BuyerType TypeOfBuyer { get; set; } = BuyerType.None;

		public Customer? Customer { get; set; }
		public int RoleId { get; set; }
		public Role Role { get; set; }
	}
}
