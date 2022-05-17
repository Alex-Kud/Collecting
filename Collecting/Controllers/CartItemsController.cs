#nullable disable
using Collecting.Data;
using Collecting.Data.Models;
using Collecting.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Collecting.Controllers
{
    /// <summary>
    /// Контроллер действий над элементами корзины
    /// </summary>
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class CartItemsController : Controller
    {
        private readonly StickersContext _context;
        private readonly User _user;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="context">Контекст БД</param>
        /// <param name="httpContextAccessor">Контекст http</param>
        public CartItemsController(StickersContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _user = JsonSerializer.Deserialize<User>(httpContextAccessor.HttpContext.Session.GetString("User"));
        }

        // GET: CartItems/CartItem/{id}
        /// <summary>
        /// Получение элемента корзины
        /// </summary>
        /// <param name="id">id элемента корзины</param>
        /// <returns>Элемент корзины</returns>
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

        // POST: CartItems/Create/{stickerId}/{quantity}
        /// <summary>
        /// Создание элемента корзины
        /// </summary>
        /// <param name="stickerId">id стикера</param>
        /// <param name="quantity">Кол-во стикеров</param>
        /// <returns>Созданный элемент корзины</returns>
        [Route("{stickerId}/{quantity}")]
        [HttpPost]
        public async Task<IActionResult> Create(int stickerId, int quantity)
        {
            if (_user == null)
            {
                return new JsonResult(new { message = "Неавторизован!" })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }

            CartItem cartItem = _context.CartItemsDb
                .Where(s => s.StickerId == stickerId && s.CartId == _user.CartId)
                .FirstOrDefault();

            if (cartItem == null || cartItem == default)
            {
                var addingSticker = _context.StickersDb
                    .Where(s => s.Id == stickerId)
                    .FirstOrDefault();

                if (addingSticker == null || addingSticker == default)
                {
                    return BadRequest("Наклейка не найдена");
                }

                if (addingSticker.Quantity < quantity)
                {
                    quantity = addingSticker.Quantity;
                }

                cartItem = new()
                {
                    Quantity = quantity,
                    StickerId = stickerId,
                    Sticker = addingSticker,
                    CartId = _user.CartId
                };
                await _context.AddAsync(cartItem);
            }
            else
            {
                cartItem.Sticker = await _context.StickersDb.FirstOrDefaultAsync(m => m.Id == cartItem.StickerId);
                cartItem.Quantity += quantity;
                if (cartItem.Sticker.Quantity < cartItem.Quantity)
                {
                    cartItem.Quantity = cartItem.Sticker.Quantity;
                }
                _context.Update(cartItem);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("CartItem", new { id = cartItem.Id }, cartItem);
        }

        // DELETE: CartItems/Delete/5
        /// <summary>
        /// Удаление элемента корзины
        /// </summary>
        /// <param name="id">id элемента корзины</param>
        /// <returns>Результат удаления</returns>
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
        /// <summary>
        /// Изменение количества товара в элементе корзины
        /// </summary>
        /// <param name="itemId">id элемента корзины</param>
        /// <param name="quantity">Новое количествотовара в элементе корзины</param>
        /// <returns>Обновленный элемент корзины</returns>
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

            cartItem.Sticker = await _context.StickersDb.FirstOrDefaultAsync(m => m.Id == cartItem.StickerId);

            if (cartItem.Sticker.Quantity < quantity)
            {
                quantity = cartItem.Sticker.Quantity;
            }
            cartItem.Quantity = quantity;

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
