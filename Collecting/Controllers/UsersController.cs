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
    public class UsersController : Controller
    {
        private readonly StickersContext _context;

        public UsersController(StickersContext context)
        {
            _context = context;
        }

        // GET: Categories/All
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> All()
        {
            return await _context.UsersDb.ToListAsync();
        }

        // GET: Users/GetUser/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.UsersDb
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: Users/Create
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

            return CreatedAtAction("User", new { id = user.Id }, user);
        }

        // DELETE: Users/Delete/5
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.UsersDb
                .FirstOrDefaultAsync(m => m.Id == id);

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
