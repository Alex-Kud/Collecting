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
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class OrdersController : Controller
    {
        private readonly StickersContext _context;
        private readonly User _user;

        public OrdersController(StickersContext context, HttpContext contextHttp)
        {
            _context = context;
            _user = (User)contextHttp.Items["User"];
        }

        // GET: All
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> All()
        {
            return await _context.OrdersDb
                .Include(o => o.Cart)
                .Include(o => o.User)
                .ToListAsync();
        }

        // GET: Orders/Order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> Order(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.OrdersDb
                .Include(o => o.Cart)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null || order == default)
            {
                return NotFound();
            }

            return order;
        }

        // POST: Orders/Create
        [HttpPost]
        public async Task<IActionResult> Create()
        {
            if (_user == null)
            {
                return new JsonResult(new { message = "Неавторизован!" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
            try
            {
                var tempCart = await _context.CartsDb.FindAsync(_user.CartId);
                if (tempCart == null)
                {
                    return NotFound();
                }
                Order order = new()
                {
                    CartId = _user.CartId,
                    Cart = tempCart,
                    UserId = _user.Id,
                    User = _user,
                    Date = DateTime.Now,
                    Status = OrderStatus.Processed
                };

                if (order != null)
                {
                    await _context.AddAsync(order);
                    await _context.SaveChangesAsync();
                }


                Cart cart = new()
                {
                    UserId = _user.Id,
                };
                await _context.CartsDb.AddAsync(cart);
                await _context.SaveChangesAsync();
                _user.CartId = cart.Id;

                _context.UsersDb.Update(_user);
                await _context.SaveChangesAsync();
                return CreatedAtAction("Order", new { id = order.Id }, order);
            }
            catch (Exception)
            {
                return BadRequest("Некорректные данные");
            }           
        }

        // Put: Orders/ChangeStatus/{id}/{newStatus}
        [HttpPut]
        [Route("{id}/{newStatus}")]
        public async Task<IActionResult> ChangeStatus(int? id, OrderStatus newStatus)
        {
            if (id == null || !Enum.IsDefined(typeof(OrderStatus), newStatus))
            {
                return NotFound();
            }

            try
            {
                Order order = await _context.OrdersDb.FindAsync(id);
                if (order == null)
                {
                    return NotFound();
                }
                order.Status = newStatus;
                _context.OrdersDb.Update(order);
                await _context.SaveChangesAsync();

                return CreatedAtAction("Order", new { id = order.Id }, order);
            }
            catch (Exception)
            {
                return BadRequest("Некорректный статус");
            }
        }

        // DELETE: Orders/Delete/5
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.OrdersDb.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            _context.OrdersDb.Remove(order);
            await _context.SaveChangesAsync();

            return new JsonResult(new { message = "Удаление заказа произошло успешно!" })
            {
                StatusCode = StatusCodes.Status200OK
            };
        }
    }
}
