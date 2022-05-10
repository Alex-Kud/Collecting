#nullable disable
using Collecting.Data;
using Collecting.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Collecting.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
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

            if (cartItem == null || cartItem == default)
            {
                return NotFound();
            }
            /*
            var answer = '[' + JsonConvert.SerializeObject(cartItem) + $",\"UnitPrice\":{cartItem.UnitPrice},\"TotalPrice\":{cartItem.TotalPrice}]";
            return Content(answer, "application/json");*/
            return cartItem;
        }

        // POST: CartItems/Create
        [Route("{StickerId}/{Quantity}")]
        [HttpPost]
        public async Task<IActionResult> Create(int StickerId, int Quantity)
        {
            User user = /*JsonSerializer.Deserialize<User>(_contextHttp.Session.GetString("User"));*/(User)_contextHttp.Items["User"];
            if (user == null)
            {
                return new JsonResult(new { message = "Неавторизован!" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }

            CartItem cartItem = await _context.CartItemsDb
                .Where(s => s.StickerId == StickerId && s.CartId == user.CartId)
                .FirstOrDefaultAsync();

            if (cartItem == null || cartItem == default)
            {
                var tempSticker = await _context.StickersDb
                    .Where(s => s.Id == StickerId)
                    .FirstOrDefaultAsync();

                if (tempSticker == null || tempSticker == default)
                {
                    return BadRequest("Наклейка не найдена");
                }

                cartItem = new()
                {
                    Quantity = Quantity,
                    StickerId = StickerId,
                    Sticker = tempSticker,
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

        // DELETE: CartItems/Delete/5
        [HttpDelete("{id}")]
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
            return new JsonResult(new { message = "Удаление элемента корзины произошло успешно!" })
            {
                StatusCode = StatusCodes.Status200OK
            };
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
