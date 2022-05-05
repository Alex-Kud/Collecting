#nullable disable
using Collecting.Data;
using Collecting.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            User user = (User)_contextHttp.Items["User"];
            if (user == null)
            {
                return new JsonResult(new { message = "Неавторизован!" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }

            CartItem cartItem = await _context.CartItemsDb
                .Where(s => s.StickerId == StickerId && s.CartId == user.CartId)
                .FirstOrDefaultAsync();

            if (cartItem == null)
            {
                cartItem = new()
                {
                    Quantity = Quantity,
                    StickerId = StickerId,
                    Sticker = await _context.StickersDb
                        .Where(s => s.Id == StickerId)
                        .FirstOrDefaultAsync(),
                    CartId = user.CartId
                };
                _context.Add(CartItem);
            }
            else
            {
                cartItem.Quantity += Quantity;
                _context.Update(cartItem);
            }

            await _context.SaveChangesAsync();
            return CreatedAtAction("CartItem", new { id = cartItem.Id }, cartItem);
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


        // POST: CartItems/ChangeQuantity/{itemId}/{quantity}
        [HttpPost]
        [Route("{itemId}/{quantity}")]
        public async Task<IActionResult> ChangeQuantity(int? itemId, int quantity)
        {
            if (itemId == null)
            {
                return NotFound();
            }

            CartItem cartItem = await _context.CartItemsDb.FindAsync(itemId);

            if (cartItem == null)
            {
                return NotFound();
            }

            cartItem.Quantity += quantity;

            if (cartItem.Quantity <= 0)
            {
                return await Delete(itemId);
            }

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

            return CreatedAtAction("CartItem", new { id = cartItem.Id }, cartItem);
        }

        private bool CartItemExists(int id) => _context.CartItemsDb.Any(e => e.Id == id);
    }
}
