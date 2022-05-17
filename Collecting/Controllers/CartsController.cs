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
    /// Контроллер действий над корзиной товаров
    /// </summary>
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class CartsController : Controller
    {
        private readonly StickersContext _context;
        private readonly User _user;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="context">Контекст БД</param>
        /// <param name="httpContextAccessor">Контекст http</param>
        public CartsController(StickersContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _user = JsonSerializer.Deserialize<User>(httpContextAccessor.HttpContext.Session.GetString("User"));
        }

        // GET: Carts/Cart/5
        /// <summary>
        /// Получение корзины по id
        /// </summary>
        /// <param name="id">id корзины</param>
        /// <returns>Корзина</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> Cart(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.CartsDb.FindAsync(id);

            if (cart == null)
            {
                return NotFound();
            }

            var items = await _context.CartItemsDb
                .Where(i => i.CartId == id)
                .ToListAsync();

            foreach (var item in items)
            {
                item.Sticker = await _context.StickersDb.FirstOrDefaultAsync(m => m.Id == item.StickerId);
            }

            cart.Items = items;

            return cart;
        }

        // GET: Carts/TotalPrice/5
        /// <summary>
        /// Получение общей стоимости корзины по id
        /// </summary>
        /// <param name="id">id корзины</param>
        /// <returns>Общая стоимость корзины</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult> TotalPrice(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var items = await _context.CartItemsDb
                .Where(i => i.CartId == id)
                .ToListAsync();

            foreach (var item in items)
            {
                item.Sticker = await _context.StickersDb.FirstOrDefaultAsync(m => m.Id == item.StickerId);
            }

            decimal total = items.Sum(i => i.TotalPrice);

            return new JsonResult(new { price = total })
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        // GET: Carts/Quantity/5
        /// <summary>
        /// Получение количества элементов в корзине по id
        /// </summary>
        /// <param name="id">id корзины</param>
        /// <returns>Количество элементов в корзине</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult> Quantity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var items = await _context.CartItemsDb
                .Where(i => i.CartId == id)
                .ToListAsync();

            return new JsonResult(new { quantity = items.Count })
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        // GET: Carts/CartUser
        /// <summary>
        /// Получение корзины авторизованного пользователя
        /// </summary>
        /// <returns>Корзина</returns>
        [HttpGet]
        public async Task<ActionResult<Cart>> CartUser()
        {
            if (_user == null)
            {
                return new JsonResult(new { message = "Неавторизован!" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }

            return await Cart(_user.CartId);
        }

        // GET: Carts/TotalPriceUser
        /// <summary>
        /// Получение общей стоимости корзины авторизованного пользователя
        /// </summary>
        /// <returns>Общая стоимость корзины</returns>
        [HttpGet]
        public async Task<ActionResult> TotalPriceUser()
        {
            if (_user == null)
            {
                return new JsonResult(new { message = "Неавторизован!" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }

            return await TotalPrice(_user.CartId);
        }

        // GET: Carts/QuantityUser
        /// <summary>
        /// Получение общего количества элементов корзины авторизованного пользователя
        /// </summary>
        /// <returns>Количество элементов корзины</returns>
        [HttpGet]
        public async Task<ActionResult> QuantityUser()
        {
            if (_user == null)
            {
                return new JsonResult(new { message = "Неавторизован!" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }

            return await Quantity(_user.CartId);

        }

        // DELETE: Carts/Delete
        /// <summary>
        /// Удаление корзины
        /// </summary>
        /// <returns>Результат удаления корзины</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            var cartItems = await _context.CartItemsDb.ToListAsync();

            if (cartItems == null)
            {
                return NotFound();
            }

            foreach (var cartItem in cartItems)
            {
                _context.CartItemsDb.Remove(cartItem);
            }
            await _context.SaveChangesAsync();
            return new JsonResult(new { message = "Очистка корзины произошла успешно!" })
            {
                StatusCode = StatusCodes.Status200OK
            };
        }
    }
}
