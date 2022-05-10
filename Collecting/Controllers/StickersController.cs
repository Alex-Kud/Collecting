#nullable disable
using Collecting.Data;
using Collecting.Data.DTO;
using Collecting.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Collecting.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class StickersController : Controller
    {
        private readonly StickersContext _context;

        public StickersController(StickersContext context)
        {
            _context = context;
        }

        // GET: Stickers/All
        [HttpGet]
        //[Authorize(Roles = "User, Admin")]
        public async Task<ActionResult<IEnumerable<StickerDTO>>> All()
        {
            var stickers = await _context.StickersDb.ToListAsync();
            var stickersDto = new List<StickerDTO>();

            foreach (var sticker in stickers)
            {
                stickersDto.Add(new StickerDTO
                {
                    Id = sticker.Id,
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
                    CategoryID = sticker.CategoryID
                });
            }

            return stickersDto;
        }

        // GET: Stickers/AllInCategory/id
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<StickerDTO>>> AllInCategory(int id)
        {
            var stickers = await _context.StickersDb
                .Where(s => s.CategoryID == id)
                .ToListAsync();

            var stickersDto = new List<StickerDTO>();

            foreach (var sticker in stickers)
            {
                stickersDto.Add(new StickerDTO
                {
                    Id = sticker.Id,
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
                    CategoryID = sticker.CategoryID
                });
            }

            return stickersDto;
        }

        // GET: Stickers/Page/{currentPage}/{pageSize}
        [Route("{currentPage}/{pageSize}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StickerDTO>>> Page(int currentPage, int pageSize)
        {
            if (currentPage <= 0 || pageSize <= 0)
            {
                return BadRequest("Некорректные данные");
            }

            decimal quantity = await _context.StickersDb.CountAsync();
            int MaxPage = (int)Math.Ceiling(quantity / pageSize);
            currentPage = currentPage > MaxPage ? MaxPage : currentPage;

            var stickers = _context.StickersDb
                .OrderBy(s => s.Id)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize);

            var stickersDto = new List<StickerDTO>();

            foreach (var sticker in stickers)
            {
                stickersDto.Add(new StickerDTO
                {
                    Id = sticker.Id,
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
                    CategoryID = sticker.CategoryID
                });
            }

            return stickersDto;
        }

        // GET: Stickers/PageInCategory/{id}/{currentPage}/{pageSize}
        [Route("{id}/{currentPage}/{pageSize}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StickerDTO>>> PageInCategory(int? id, int currentPage, int pageSize)
        {
            if (currentPage <= 0 || pageSize <= 0 || id == null)
            {
                return BadRequest("Некорректные данные");
            }

            decimal quantity = await _context.StickersDb.CountAsync();
            int MaxPage = (int)Math.Ceiling(quantity / pageSize);
            currentPage = currentPage > MaxPage ? MaxPage : currentPage;

            IQueryable<Sticker> stickers;
            if (id == 0)
            {
                stickers = _context.StickersDb
                .OrderBy(s => s.Id)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize);
            }
            else
            {
                stickers = _context.StickersDb
                .Where(s => s.CategoryID == id)
                .OrderBy(s => s.Id)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize);
            }

            var stickersDto = new List<StickerDTO>();

            foreach (var sticker in stickers)
            {
                stickersDto.Add(new StickerDTO
                {
                    Id = sticker.Id,
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
                    CategoryID = sticker.CategoryID
                });
            }

            return stickersDto;
        }

        // GET: Stickers/Sticker/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Sticker(int? id)
        {
            /*var temp = _context.StickersDb.ToList();
            foreach (var sticker in temp)
            {
                _context.StickersDb.Remove(sticker);
            }

            // Костыль. Необходимо сделать нормальное получение строки подключения к БД из файла конфигурации
            var db_options = new DbContextOptionsBuilder<StickersContext>()
                .UseSqlServer("Server = (localdb)\\mssqllocaldb; Database = Collection; Trusted_Connection = True; MultipleActiveResultSets = true")
                .Options;
            StickersContext context = new StickersContext(db_options);
            */
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var sticker = await _context.StickersDb
                    .FirstOrDefaultAsync(m => m.Id == id);

                var category = await _context.CategoriesDb.
                    Where(c => c.Id == sticker.CategoryID).
                    FirstOrDefaultAsync();

                if (sticker == null || category == null)
                {
                    return BadRequest("Наклейка не найдена");
                }

                StickerDTO stickerDTO = new()
                {
                    Id = sticker.Id,
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
                    CategoryID = sticker.CategoryID
                };

                CategoryDTO categoryDTO = new()
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                };

                var answer = '[' + JsonConvert.SerializeObject(stickerDTO) + ',' + JsonConvert.SerializeObject(categoryDTO) + ']';

                return Content(answer, "application/json");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        // GET: Stickers/Search/text
        [HttpGet("{text}")]
        public async Task<ActionResult<IEnumerable<StickerDTO>>> Search(string text)
        {
            var stickers = await _context.StickersDb.ToListAsync();
            var stickersDto = new List<StickerDTO>();

            foreach (var sticker in stickers)
            {
                if (sticker.Text.ToUpper().Contains(text.ToUpper()) || sticker.Firm.ToUpper().Contains(text.ToUpper()))
                {
                    stickersDto.Add(new StickerDTO
                    {
                        Id = sticker.Id,
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
                        CategoryID = sticker.CategoryID
                    });
                }
            }

            return stickersDto;
        }

        // GET: Stickers/SearchQuantity/text
        [HttpGet("{text}")]
        public async Task<ActionResult<IEnumerable<StickerDTO>>> SearchQuantity(string text)
        {
            var stickers = await _context.StickersDb.ToListAsync();
            int counter = 0;

            foreach (var sticker in stickers)
            {
                if (sticker.Text.ToUpper().Contains(text.ToUpper()) || sticker.Firm.ToUpper().Contains(text.ToUpper()))
                {
                    counter++;
                }
            }

            return new JsonResult(new { quantity = counter })
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        // GET: Stickers/AllQuantityInCategory
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<StickerDTO>>> AllQuantityInCategory(int id)
        {
            int quant = 0;
            if (id == 0)
            {
                quant = await _context.StickersDb.CountAsync();
            }
            else
            {
                 quant = await _context.StickersDb
                 .Where(s => s.CategoryID == id)
                 .CountAsync();
            }
            return new JsonResult(new { quantity = quant})
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        // GET: Stickers/AllQuantityInCategory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StickerDTO>>> AllQuantity()
        {
            return new JsonResult(new { quantity = await _context.StickersDb.CountAsync() })
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        // POST: Stickers/Create
        [HttpPost]
        public async Task<IActionResult> Create(StickerDTO stickerDTO)
        {
            Sticker sticker = new()
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
                CategoryID = stickerDTO.CategoryID
            };

            if (sticker != null && CategoryExists(sticker.CategoryID))
            {
                await _context.AddAsync(sticker);
                await _context.SaveChangesAsync();
                return CreatedAtAction("Sticker", new { id = sticker.Id }, sticker);
            }

            return BadRequest("Некорректные данные");
        }

        // POST: Stickers/Edit/5
        [HttpPost("{id}")]
        public async Task<IActionResult> Edit(int id, StickerDTO stickerDto)
        {
            if (id != stickerDto.Id || stickerDto == null)
            {
                return NotFound();
            }

            if (!CategoryExists(stickerDto.CategoryID))
            {
                return BadRequest("Категория не существует");
            }

            Sticker sticker = new()
            {
                Id = stickerDto.Id,
                Firm = stickerDto.Firm,
                Year = stickerDto.Year,
                Country = stickerDto.Country,
                Material = stickerDto.Material,
                Width = stickerDto.Width,
                Height = stickerDto.Height,
                Text = stickerDto.Text,
                Quantity = stickerDto.Quantity,
                Price = stickerDto.Price,
                Form = stickerDto.Form,
                Img = stickerDto.Img,
                AdditionalImg = stickerDto.AdditionalImg,
                CategoryID = stickerDto.CategoryID
            };

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

            return CreatedAtAction("Sticker", new { id = sticker.Id }, sticker);
        }

        // Delete: Stickers/Delete/5
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest("Наклейка не найдена");
            }
            var sticker = await _context.StickersDb.FindAsync(id);
            if (sticker == null)
            {
                return BadRequest("Наклейка не найдена");
            }
            _context.StickersDb.Remove(sticker);
            await _context.SaveChangesAsync();
            return new JsonResult(new { message = "Удаление произошло успешно!" }) { 
                StatusCode = StatusCodes.Status200OK 
            };
        }

        private bool StickerExists(int id) => _context.StickersDb.Any(e => e.Id == id);

        private bool CategoryExists(int id) => _context.CategoriesDb.Any(e => e.Id == id);
    }
}
