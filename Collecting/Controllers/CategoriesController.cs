#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Collecting.Data;
using Collecting.Models;
using Collecting.Data.DTO;

namespace Collecting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CategoriesController : Controller
    {
        private readonly StickersContext _context;

        public CategoriesController(StickersContext context)
        {
            _context = context;
        }

        // POST: Categories/Create
        [HttpPost]
        public async Task<IActionResult> Create( CategoryDTO categoryDTO)
        {
            Category category = new Category();
            
                category.Name = categoryDTO.Name;
                category.Description = categoryDTO.Description;
            

            if (category != null)
            {
                await _context.AddAsync(category);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetCategory", new { id = category.Id }, category);
            }
            return BadRequest("Некорректные данные");
        }

        // GET: Categories/GetCategory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var category = await _context.CategoriesDb
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (category == null)
                {
                    return NotFound();
                }

                var categoryDTO = new CategoryDTO
                {
                    Name = category.Name,
                    Description = category.Description
                };

                return categoryDTO;
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        /*
        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.CategoriesDb.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.CategoriesDb
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.CategoriesDb.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.CategoriesDb
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.CategoriesDb.FindAsync(id);
            _context.CategoriesDb.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.CategoriesDb.Any(e => e.Id == id);
        }*/
    }
}
