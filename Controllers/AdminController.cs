using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TaskAuthenticationAuthorization.Models
{
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : Controller
    {
        private readonly ShoppingContext _context;

        public AdminController(ShoppingContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.Include(r => r.Role).ToListAsync());
        }

        [AllowAnonymous]
        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                                     .Include(r => r.Role)
                                     .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewBag.BuyerTypes = FillViewBagForTypeOfBuyer(BuyerType.None);
            ViewBag.Roles = FillViewBagForRoles(_context.Roles.First(r => r.Name == "buyer").RoleId);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,TypeOfBuyer,RoleId")] User user)
        {
            if (ModelState.IsValid)
            {
                User chekUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (chekUser == null)
                {
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Email", "This email is already taken");
                }
            }

            ViewBag.BuyerTypes = FillViewBagForTypeOfBuyer(user.TypeOfBuyer);
            ViewBag.Roles = FillViewBagForRoles(user.RoleId);

            return View(user);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.BuyerTypes = FillViewBagForTypeOfBuyer(user.TypeOfBuyer);
            ViewBag.Roles = FillViewBagForRoles(user.RoleId);

            return View(user);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var users = _context.Users.Where(u => u.Email == user.Email);
                    bool idValid = false;
                    if (users.Count() == 0)
                    {
                        idValid = true;
                    }
                    else if (users.Count() == 1 & users.First().Id == id)
                    {
                        idValid = true;
                    }
                    if (idValid)
                    {
                        var existingUser = await _context.Users.FindAsync(id);
                        existingUser.RoleId = user.RoleId;
                        existingUser.Email = user.Email;
                        existingUser.TypeOfBuyer = user.TypeOfBuyer;
                        _context.Update(existingUser);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "This email is already taken");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewBag.BuyerTypes = FillViewBagForTypeOfBuyer(user.TypeOfBuyer);
            ViewBag.Roles = FillViewBagForRoles(user.RoleId);

            return View(user);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool ProductExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
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
        private dynamic FillViewBagForRoles(int id)
        {
            return _context.Roles
                           .Select(role => new SelectListItem
                           {
                               Text = role.Name,
                               Value = role.RoleId.ToString(),
                               Selected = role.RoleId == id
                           })
                           .ToList();
        }
    }
}
