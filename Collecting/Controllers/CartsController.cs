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
    public class CartsController : Controller
    {
        private readonly StickersContext _context;
        private readonly User _user;

        public CartsController(StickersContext context, HttpContext contextHttp)
        {
            _context = context;
            //_user = JsonSerializer.Deserialize<User>(contextHttp.Session.GetString("User"));
            _user = (User) contextHttp.Items["User"];
        }
        
        // GET: Carts/Cart/5
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
            cart.Items = items;

            return cart;
        }

        // GET: Carts/TotalPrice/5
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

            decimal total = items.Sum(i => i.TotalPrice);

            return new JsonResult(new { price = total })
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        // GET: Carts/Quantity/5
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
        [HttpGet]
        public async Task<ActionResult> QuantityUser()
        {
            if (_user == null)
            {
                return new JsonResult(new { message = "Неавторизован!" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }

            return await TotalPrice(_user.CartId);

        }
    }
}
