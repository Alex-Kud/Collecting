#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Collecting.Data;
using Collecting.Data.Models;

namespace Collecting.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    public class CartItemsController : Controller
    {
        private readonly StickersContext _context;
        private readonly HttpContext _contextHttp;

        public CartItemsController(StickersContext context, HttpContext contextHttp)
        {
            _context = context;
            _contextHttp = contextHttp;
        }

        // GET: CartItems/CartItem/{id}
        [HttpGet]
        public async Task<ActionResult<CartItem>> CartItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItemsDb
                .Include(c => c.Sticker)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cartItem == null)
            {
                return NotFound();
            }

            return cartItem;
        }

        // POST: CartItems/Create
        [Route("{StickerId}/{Quantity}")]
        [HttpPost]
        public async Task<IActionResult> Create(int StickerId, int Quantity)
        {
            var user = _contextHttp.Items["User"];
            if (user == null)
            {
                return new JsonResult(new { message = "Неавторизован!" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }

            //CartItem current = await _context.CartItemsDb.Where(s => s.StickerId == StickerId && s.CartId = cartId).FirstOrDefaultAsync();
            CartItem CartItem = new()
            {
                Quantity = Quantity,
                StickerId = StickerId,
                Sticker = await _context.StickersDb.Where(s => s.Id == StickerId).FirstOrDefaultAsync()
                // CartId берем из сессии по авторизованному пользователю
            };

            _context.Add(CartItem);
            await _context.SaveChangesAsync();
            return CreatedAtAction("CartItem", new { id = CartItem.Id }, CartItem);
        }

        // POST: CartItems/Delete/5
        [HttpPost("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItemsDb.FindAsync(id);

            if (cartItem == null)
            {
                return NotFound();
            }

            _context.CartItemsDb.Remove(cartItem);
            await _context.SaveChangesAsync();
            return CreatedAtAction("CartItem", new { id = cartItem.Id }, cartItem);
        }



        /*
        // GET: CartItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItemsDb.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }
            ViewData["CartId"] = new SelectList(_context.CartsDb, "Id", "Id", cartItem.CartId);
            ViewData["StickerId"] = new SelectList(_context.StickersDb, "Id", "Id", cartItem.StickerId);
            return View(cartItem);
        }

        // POST: CartItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Quantity,StickerId,CartId")] CartItem cartItem)
        {
            if (id != cartItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cartItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartItemExists(cartItem.Id))
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
            ViewData["CartId"] = new SelectList(_context.CartsDb, "Id", "Id", cartItem.CartId);
            ViewData["StickerId"] = new SelectList(_context.StickersDb, "Id", "Id", cartItem.StickerId);
            return View(cartItem);
        }

        private bool CartItemExists(int id)
        {
            return _context.CartItemsDb.Any(e => e.Id == id);
        }*/
    }
}
