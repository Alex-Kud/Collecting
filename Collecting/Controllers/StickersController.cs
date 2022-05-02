﻿#nullable disable
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
using Newtonsoft.Json;
using System.Configuration;

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

        // POST: Stickers/Create
        [HttpPost]
        public async Task<IActionResult> Create(StickerDTO stickerDTO)
        {
            Sticker sticker = new Sticker
            {
                Firm = stickerDTO.Firm,
                Year = stickerDTO.Year,
                Country = stickerDTO.Country,
                Material = stickerDTO.Material,
                Width = stickerDTO.Width,
                Height = stickerDTO.Height,
                Text = stickerDTO.Text,
                Quantity = stickerDTO.Quantity,
                Price = stickerDTO.Price,
                Form = stickerDTO.Form,
                Img = stickerDTO.Img,
                AdditionalImg = stickerDTO.AdditionalImg,
                categoryID = stickerDTO.categoryID
            };

            if (sticker != null)
            {
                await _context.AddAsync(sticker);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetSticker", new { id = sticker.Id }, sticker);
            }

            return BadRequest("Некорректные данные");
        }

        // GET: Stickers/Details/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetSticker(int? id)
        {
            // Костыль. Необходимо сделать нормальное получение строки подключения к БД из файла конфигурации
            var db_options = new DbContextOptionsBuilder<StickersContext>()
                .UseSqlServer("Server = (localdb)\\mssqllocaldb; Database = Collection; Trusted_Connection = True; MultipleActiveResultSets = true")
                .Options;
            StickersContext context = new StickersContext(db_options);

            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var sticker = await context.StickersDb
                    .FirstOrDefaultAsync(m => m.Id == id);

                var category = await _context.CategoriesDb.
                    Where(c => c.Id == sticker.categoryID).
                    FirstOrDefaultAsync();

                if (sticker == null || category == null)
                {
                    return BadRequest("Наклейка не найдена");
                }

                StickerDTO stickerDTO = new StickerDTO
                {
                    Firm = sticker.Firm,
                    Year = sticker.Year,
                    Country = sticker.Country,
                    Material = sticker.Material,
                    Width = sticker.Width,
                    Height = sticker.Height,
                    Text = sticker.Text,
                    Quantity = sticker.Quantity,
                    Price = sticker.Price,
                    Form = sticker.Form,
                    Img = sticker.Img,
                    AdditionalImg = sticker.AdditionalImg,
                    categoryID = sticker.categoryID
                };

                CategoryDTO categoryDTO = new CategoryDTO
                {
                    Name = category.Name,
                    Description = category.Description
                };

                var answer = '[' + JsonConvert.SerializeObject(stickerDTO) + ',' + JsonConvert.SerializeObject(categoryDTO) + ']';

                return Content(answer, "application/json");
            }
            catch (Exception ex)
            {
                return NotFound();
            }
            
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
        *//*
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
        *//*
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
        }
        */
    }
}
