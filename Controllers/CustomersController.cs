using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskAuthenticationAuthorization.Models;
using TaskAuthenticationAuthorization.ViewModels;

namespace TaskAuthenticationAuthorization.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class CustomersController : Controller
    {
        private readonly ShoppingContext _context;

        public CustomersController(ShoppingContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {

            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["AddressSortParam"] = sortOrder == "Address" ? "address_desc" : "Address";
            ViewData["CurrentFilter"] = searchString;
            var customers = from s in _context.Customers
                            select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                customers = customers.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    customers = customers.OrderByDescending(s => s.LastName);
                    break;
                case "Address":
                    customers = customers.OrderBy(s => s.Address);
                    break;
                case "address_desc":
                    customers = customers.OrderByDescending(s => s.Address);
                    break;
                default:
                    customers = customers.OrderBy(s => s.LastName);
                    break;
            }

            var shoppingContext = customers.Include(c => c.User)
                                           .ThenInclude(u => u.Role);

            return View(await shoppingContext.AsNoTracking().ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            ViewBag.Discount = FillViewBagForDiscount(Discount.R);
            ViewBag.TypeOfBuyer = FillViewBagForTypeOfBuyer(BuyerType.Regular);

            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerViewModel customer)
        {
            if (ModelState.IsValid)
            {
                User chekUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == customer.Email);
                if (chekUser == null)
                {
                    User user = new User
                    {
                        Email = customer.Email,
                        Password = customer.Password,
                        TypeOfBuyer = customer.TypeOfBuyer
                    };

                    Role userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "buyer");
                    if (userRole != null)
                        user.Role = userRole;
                    _context.Add(user);
                    await _context.SaveChangesAsync();

                    Customer newCustomer = new Customer
                    {
                        UserId = user.Id,
                        LastName = customer.LastName,
                        FirstName = customer.FirstName,
                        Address = customer.Address,
                        Discount = customer.Discount,
                    };
                    _context.Add(newCustomer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Email", "This email is already taken");
                }
            }
            ViewBag.Discount = FillViewBagForDiscount(customer.Discount);
            ViewBag.TypeOfBuyer = FillViewBagForTypeOfBuyer(customer.TypeOfBuyer);

            return View(customer);
        }



        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            User chekUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == customer.UserId);

            CustomerViewModel editedCustomer = new CustomerViewModel
            {
                LastName = customer.LastName,
                FirstName = customer.FirstName,
                Email = chekUser.Email,
                Address = customer.Address,
                TypeOfBuyer = chekUser.TypeOfBuyer,
                Discount = customer.Discount
            };

            ViewBag.Discount = FillViewBagForDiscount(editedCustomer.Discount);
            ViewBag.TypeOfBuyer = FillViewBagForTypeOfBuyer(editedCustomer.TypeOfBuyer);

            return View(editedCustomer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CustomerViewModel customer)
        {
            Customer editedCustomer = await _context.Customers.FirstOrDefaultAsync(u => u.Id == id);
            if (editedCustomer is null)
            {
                return NotFound();
            }

            ModelState.Remove("Email");     // This will remove the key 
            ModelState.Remove("Password");  // This will remove the key 

            if (ModelState.IsValid)
            {
                try
                {
                    User editedUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == customer.Email);
                    editedUser.TypeOfBuyer = customer.TypeOfBuyer;
                    _context.Update(editedUser);

                    editedCustomer.FirstName = customer.FirstName;
                    editedCustomer.LastName = customer.LastName;
                    editedCustomer.Address = customer.Address;
                    editedCustomer.Discount = customer.Discount;
                    _context.Update(editedCustomer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Discount = FillViewBagForDiscount(customer.Discount);
            ViewBag.TypeOfBuyer = FillViewBagForTypeOfBuyer(customer.TypeOfBuyer);

            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }

        private dynamic FillViewBagForDiscount(Discount? discount)
        {

            return Enum.GetValues(typeof(Discount))
                        .Cast<Discount>()
                        .Select(v => new SelectListItem
                        {
                            Text = v.ToString(),
                            Value = v.ToString(),
                            Selected = v == (discount ?? Discount.R)
                        })
                        .ToList();

        }
        private dynamic FillViewBagForTypeOfBuyer(BuyerType buyerType)
        {
            return Enum.GetValues(typeof(BuyerType))
                       .Cast<BuyerType>()
                       .Select(v => new SelectListItem
                       {
                           Text = v.ToString(),
                           Value = v.ToString(),
                           Selected = v == buyerType
                       })
                       .ToList();
        }
    }
}
