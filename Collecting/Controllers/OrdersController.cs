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
using System.Text.Json;
using Collecting.Middleware;

namespace Collecting.Controllers
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class OrdersController : Controller
    {
        private readonly StickersContext _context;
        private readonly User _user;

        public OrdersController(StickersContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _user = JsonSerializer.Deserialize<User>(httpContextAccessor.HttpContext.Session.GetString("User"));
        }

        // GET: All
        /// <summary>
        /// Получение всех заказов
        /// </summary>
        /// <returns>Список всех заказов</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> All()
        {
            var orders = await _context.OrdersDb
                .Include(o => o.Cart)
                .Include(o => o.User)
                .ToListAsync();

            foreach (var order in orders)
            {
                order.Cart.Items = await _context.CartItemsDb.Where(i => i.CartId == order.CartId).ToListAsync();
                foreach (var item in order.Cart.Items)
                {
                    item.Sticker = await _context.StickersDb.FindAsync(item.StickerId);
                }
            }

            return orders;
        }

        // GET: Orders/Order/5
        /// <summary>
        /// Получение заказа по id
        /// </summary>
        /// <param name="id">id заказа</param>
        /// <returns>Заказ с заданным id</returns>
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

        // GET: Orders/UserOrders
        /// <summary>
        /// Получение заказов авторизованного пользователя
        /// </summary>
        /// <returns>Список заказов авторизованного пользователя</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> UserOrders()
        {
            if (_user == null)
            {
                return new JsonResult(new { message = "Неавторизован!" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }

            var orders = await _context.OrdersDb
                .Include(o => o.Cart)
                .Include(o => o.User)
                .Where(o => o.UserId == _user.Id)
                .ToListAsync();

            if (orders == null || orders == default)
            {
                return NotFound();
            }

            foreach (var order in orders)
            {
                order.Cart.Items = await _context.CartItemsDb.Where(i => i.CartId == order.CartId).ToListAsync();
                foreach (var item in order.Cart.Items)
                {
                    item.Sticker = await _context.StickersDb.FindAsync(item.StickerId);
                }
            }
            orders.OrderBy(o => o.Status);
            return orders;
        }

        // POST: Orders/Create
        /// <summary>
        /// Создание заказа
        /// </summary>
        /// <returns>Созданный заказ</returns>
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
                    //User = _user,
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

                User user = await _context.UsersDb.FindAsync(_user.Id);
                user.CartId = cart.Id;
                _context.UsersDb.Update(user);
                await _context.SaveChangesAsync();
                return CreatedAtAction("Order", new { id = order.Id }, order);
            }
            catch (Exception)
            {
                return BadRequest("Некорректные данные");
            }           
        }

        // Put: Orders/ChangeStatus/{id}/{newStatus}
        /// <summary>
        /// Изменение статуса заказа
        /// </summary>
        /// <param name="id">id заказа</param>
        /// <param name="newStatus">Новый статус</param>
        /// <returns>Измененный заказ</returns>
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
        /// <summary>
        /// Удаление заказа
        /// </summary>
        /// <param name="id">id заказа</param>
        /// <returns>Сообщение о том, удалось ли удалить заказ</returns>
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
