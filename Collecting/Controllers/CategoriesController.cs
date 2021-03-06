#nullable disable
using Collecting.Data;
using Collecting.Data.DTO;
using Collecting.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Collecting.Controllers
{
    /// <summary>
    /// Контроллер действий над категориями
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class CategoriesController : Controller
    {
        private readonly StickersContext _context;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="context">Контекст БД</param>
        public CategoriesController(StickersContext context)
        {
            _context = context;
        }

        // GET: Categories/All
        /// <summary>
        /// Получение всех категорий
        /// </summary>
        /// <returns>Список категорий</returns>
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> All()
        {
            var categories = await _context.CategoriesDb.ToListAsync();
            var categoriesDto = new List<CategoryDTO>();
            foreach (var category in categories)
            {
                categoriesDto.Add(new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                });
            }

            return categoriesDto;
        }

        // GET: Categories/Category/5
        /// <summary>
        /// Получение категоии по id
        /// </summary>
        /// <param name="id">id категории</param>
        /// <returns>Категория</returns>
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

                if (category == null || category == default)
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
            catch (Exception)
            {
                return NotFound();
            }
        }

        // POST: Categories/Create
        /// <summary>
        /// Создание категории
        /// </summary>
        /// <param name="categoryDTO">Данные категории</param>
        /// <returns>Созданная категория</returns>
        [HttpPost]
        public async Task<IActionResult> Create(CategoryDTO categoryDTO)
        {
            Category category = new()
            {
                Name = categoryDTO.Name,
                Description = categoryDTO.Description
            };

            if (category != null)
            {
                await _context.AddAsync(category);
                await _context.SaveChangesAsync();
                return CreatedAtAction("Category", new { id = category.Id }, category);
            }
            return BadRequest("Некорректные данные");
        }

        // POST: Categories/Edit/5
        /// <summary>
        /// Редактирование категории
        /// </summary>
        /// <param name="id">id категории</param>
        /// <param name="categoryDto">Новые данные категории</param>
        /// <returns>Измененная категория</returns>
        [HttpPost("{id}")]
        public async Task<IActionResult> Edit(int id, CategoryDTO categoryDto)
        {
            if (id != categoryDto.Id || categoryDto == null)
            {
                return NotFound();
            }

            Category category = new()
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

            return CreatedAtAction("Category", new { id = category.Id }, category);
        }

        // POST: Categories/Delete/5
        /// <summary>
        /// Удаление категории
        /// </summary>
        /// <param name="id">id категории</param>
        /// <returns>Результат удаления</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest("Категория не найдена");
            }
            var category = await _context.CategoriesDb.FindAsync(id);
            if (category == null)
            {
                return BadRequest("Категория не найдена");
            }
            _context.CategoriesDb.Remove(category);
            await _context.SaveChangesAsync();
            return new JsonResult(new { message = "Удаление категории произошло успешно!" }) { StatusCode = StatusCodes.Status200OK };
        }

        private bool CategoryExists(int id) => _context.CategoriesDb.Any(e => e.Id == id);
    }
}
