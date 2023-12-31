﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskAuthenticationAuthorization.Models;

namespace TaskAuthenticationAuthorization.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class OrdersController : Controller
    {
        private readonly ShoppingContext _context;

        public OrdersController(ShoppingContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        // GET: Orders
        public async Task<IActionResult> Index()
        {
            if (User.HasClaim(ClaimsIdentity.DefaultRoleClaimType, "buyer"))
            {
                string email = User.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value;
                int userId = _context.Users.FirstOrDefault(u => u.Email == email).Id;
                var shoppingContext = _context.Orders
                                                    .Include(o => o.Customer)
                                                    .Include(o => o.SuperMarket)
                                                    .Where(c => c.Customer.UserId == userId);
                return View(await shoppingContext.ToListAsync());
            }
            else if (User.HasClaim(ClaimsIdentity.DefaultRoleClaimType, "admin"))
            {
                var shoppingContext = _context.Orders.Include(o => o.Customer).Include(o => o.SuperMarket);
                return View(await shoppingContext.ToListAsync());
            }
            return NotFound();
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.SuperMarket)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            if (User.HasClaim(ClaimsIdentity.DefaultRoleClaimType, "buyer"))
            {
                string email = User.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value;
                int userId = _context.Users.FirstOrDefault(u => u.Email == email).Id;
                if(order.Customer.UserId != userId)
                {
                    return NotFound();
                }
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id");
            ViewData["SuperMarketId"] = new SelectList(_context.SuperMarkets, "SuperMarketId", "SuperMarketId");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderDate,CustomerId,SuperMarketId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", order.CustomerId);
            ViewData["SuperMarketId"] = new SelectList(_context.SuperMarkets, "SuperMarketId", "SuperMarketId", order.SuperMarketId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", order.CustomerId);
            ViewData["SuperMarketId"] = new SelectList(_context.SuperMarkets, "SuperMarketId", "SuperMarketId", order.SuperMarketId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderDate,CustomerId,SuperMarketId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", order.CustomerId);
            ViewData["SuperMarketId"] = new SelectList(_context.SuperMarkets, "SuperMarketId", "SuperMarketId", order.SuperMarketId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.SuperMarket)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
