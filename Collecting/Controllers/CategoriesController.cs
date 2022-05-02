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

namespace Collecting.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    public class CategoriesController : Controller
    {
        private readonly StickersContext _context;

        public CategoriesController(StickersContext context)
        {
            _context = context;
        }

        // GET: Categories/All
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> All()
        {
            var categories = await _context.CategoriesDb.ToListAsync();
            var categoriesDto = new List<CategoryDTO>();
            foreach (var category in categories)
            {
                categoriesDto.Add(new CategoryDTO { Id = category.Id, Name = category.Name, Description = category.Description });
            }

            return categoriesDto;
        }

        // GET: Categories/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> Category(int? id)
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
                    Id = category.Id,
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

        // POST: Categories/Create
        [HttpPost]
        public async Task<IActionResult> Create(CategoryDTO categoryDTO)
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

        // POST: Categories/Edit/5
        [HttpPost("{id}")]
        public async Task<IActionResult> Edit(int id, CategoryDTO categoryDto)
        {
            if (id != categoryDto.Id || categoryDto == null)
            {
                return NotFound();
            }

            Category category = new Category
            {
                Id = categoryDto.Id,
                Name = categoryDto.Name,
                Description = categoryDto.Description
            };

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

            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        }

        // POST: Categories/Delete/5
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.CategoriesDb.FindAsync(id);
            _context.CategoriesDb.Remove(category);
            await _context.SaveChangesAsync();
            return new JsonResult(new { message = "Удаление произошло успешно!" }) { StatusCode = StatusCodes.Status200OK };
        }
        
        private bool CategoryExists(int id)
        {
            return _context.CategoriesDb.Any(e => e.Id == id);
        }
    }
}
