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
    public class UsersController : Controller
    {
        private readonly StickersContext _context;
        private readonly User _user;

        public UsersController(StickersContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _user = JsonSerializer.Deserialize<User>(httpContextAccessor.HttpContext.Session.GetString("User"));
        }

        // GET: Users/All
        /// <summary>
        /// Получение списка всех пользователей
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> All()
        {
            return await _context.UsersDb.ToListAsync();
        }

        // GET: Users/GetUser/5
        /// <summary>
        /// Получение данных о пользователе по его id
        /// </summary>
        /// <param name="id">id пользователя</param>
        /// <returns>Пользователь</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.UsersDb
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null || user == default)
            {
                return NotFound();
            }

            return user;
        }

        // GET: Users/GetUser
        /// <summary>
        /// Получение данных об авторизованном пользователе
        /// </summary>
        /// <returns>Пользователь</returns>
        [HttpGet]
        public async Task<ActionResult<User>> GetUser()
        {
            return await GetUser(_user.Id);
        }

        // POST: Users/Create
        /// <summary>
        /// Создание пользователя
        /// </summary>
        /// <param name="user">Данные пользователя</param>
        /// <returns>Созданный пользователь</returns>
        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (user == null)
            {
                return BadRequest("Некорректные данные");
            }

            Cart cart = new()
            {
                UserId = user.Id,
            };
            await _context.CartsDb.AddAsync(cart);
            await _context.SaveChangesAsync();
            user.CartId = cart.Id;

            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // Put: Users/ChangeRole/{id}/{newRole}
        /// <summary>
        /// Изменение роли пользователя
        /// </summary>
        /// <param name="id">id пользователя</param>
        /// <param name="newRole">Новая роль</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}/{newRole}")]
        public async Task<IActionResult> ChangeRole(int? id, Roles newRole)
        {
            if (id == null || !Enum.IsDefined(typeof(Roles), newRole))
            {
                return NotFound();
            }

            try
            {
                User user = await _context.UsersDb.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                user.Role = newRole;
                _context.UsersDb.Update(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetUser", new { id = user.Id }, user);
            }
            catch (Exception)
            {
                return BadRequest("Некорректная роль");
            }
        }

        // DELETE: Users/Delete/5
        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="id">id пользователя</param>
        /// <returns>Сообщение с результатом удаление (успешно или нет)</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.UsersDb.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.UsersDb.Remove(user);
            await _context.SaveChangesAsync();

            return new JsonResult(new { message = "Удаление пользователя произошло успешно!" })
            {
                StatusCode = StatusCodes.Status200OK
            };
        }


        /*
        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.UsersDb.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,Country,Address,Index,Email,Password,Role,CartId")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        private bool UserExists(int id)
        {
            return _context.UsersDb.Any(e => e.Id == id);
        }*/
    }
}
