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

namespace Collecting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class StickersController : Controller
    {
        private readonly StickersContext _context;

        public StickersController(StickersContext context)
        {
            _context = context;
        }
/*
        // GET: Stickers
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var stickersContext = _context.StickersDb.Include(s => s.Category);
            return View(await stickersContext.ToListAsync());
        }

        // GET: Stickers/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sticker = await _context.StickersDb
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sticker == null)
            {
                return NotFound();
            }

            return View(sticker);
        }

        // GET: Stickers/Create
        public IActionResult Create()
        {
            ViewData["categoryID"] = new SelectList(_context.CategoriesDb, "Id", "Id");
            return View();
        }

        // POST: Stickers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Firm,Year,Country,Material,Width,Height,Text,Quantity,Price,Form,Img,AdditionalImg,categoryID")] Sticker sticker)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sticker);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["categoryID"] = new SelectList(_context.CategoriesDb, "Id", "Id", sticker.categoryID);
            return View(sticker);
        }

        // GET: Stickers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sticker = await _context.StickersDb.FindAsync(id);
            if (sticker == null)
            {
                return NotFound();
            }
            ViewData["categoryID"] = new SelectList(_context.CategoriesDb, "Id", "Id", sticker.categoryID);
            return View(sticker);
        }

        // POST: Stickers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Firm,Year,Country,Material,Width,Height,Text,Quantity,Price,Form,Img,AdditionalImg,categoryID")] Sticker sticker)
        {
            if (id != sticker.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sticker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StickerExists(sticker.Id))
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
            ViewData["categoryID"] = new SelectList(_context.CategoriesDb, "Id", "Id", sticker.categoryID);
            return View(sticker);
        }

        // GET: Stickers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sticker = await _context.StickersDb
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sticker == null)
            {
                return NotFound();
            }

            return View(sticker);
        }

        // POST: Stickers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sticker = await _context.StickersDb.FindAsync(id);
            _context.StickersDb.Remove(sticker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StickerExists(int id)
        {
            return _context.StickersDb.Any(e => e.Id == id);
        }*/
    }
}
