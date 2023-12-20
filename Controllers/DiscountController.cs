using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TaskAuthenticationAuthorization.Models;
using TaskAuthenticationAuthorization.ViewModels;

namespace TaskAuthenticationAuthorization.Controllers
{
	//[Authorize(Policy = "RestrictionForBuyerType")] //For all Discount types 
	[Authorize(Policy = "RestrictionForBuyerType_are_Golden_or_Wholesale")]
	public class DiscountController : Controller
	{
		private readonly ShoppingContext _context;

		public DiscountController(ShoppingContext context)
		{
			_context = context;
		}
		public async Task<IActionResult> Index()
		{
			var userEmail = User.Identity.Name;

			int userId = _context.Users.FirstOrDefault(u => u.Email == userEmail).Id;

			var user = await _context.Users
				.Include(u => u.Customer)
				.Where(c => c.Id == userId)
				.FirstOrDefaultAsync();

			var customer = new DiscountViewModel
			{
				LastName = user.Customer.LastName,
				FirstName = user.Customer.FirstName,
				Email = userEmail,
				Address = user.Customer.Address,
				Discount = user.Customer.Discount.ToString(),
				TypeOfBuyer = user.TypeOfBuyer.ToString()
			};

			return View(customer);
		}
	}
}
