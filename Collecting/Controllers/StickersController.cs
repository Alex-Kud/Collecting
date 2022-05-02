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
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    public class StickersController : Controller
    {
        private readonly StickersContext _context;

        public StickersController(StickersContext context)
        {
            _context = context;
        }


        // GET: Stickers/All
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StickerDTO>>> All()
        {
            var stickers = await _context.StickersDb.ToListAsync();
            var stickersDto = new List<StickerDTO>();

            foreach (var sticker in stickers)
            {
                stickersDto.Add(new StickerDTO {
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
                    categoryID = sticker.categoryID
                });
            }

            return stickersDto;
        }

        // GET: Stickers/allincategory/id
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<StickerDTO>>> AllInCategory(int id)
        {
            var stickers = await _context.StickersDb.Where(s => s.categoryID == id).ToListAsync();
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
                    categoryID = sticker.categoryID
                });
            }

            return stickersDto;
        }

        // GET: Stickers/Sticker/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Sticker(int? id)
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
                    categoryID = sticker.categoryID
                };

                CategoryDTO categoryDTO = new CategoryDTO
                {
                    Id = category.Id,
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

        // POST: Stickers/Create
        [HttpPost]
        public async Task<IActionResult> Create(StickerDTO stickerDTO)
        {
            Sticker sticker = new Sticker {
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

            if (sticker != null && CategoryExists(sticker.categoryID))
            {
                await _context.AddAsync(sticker);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetSticker", new { id = sticker.Id }, sticker);
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

            Sticker sticker = new Sticker
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
                categoryID = stickerDto.categoryID
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

            return CreatedAtAction("GetSticker", new { id = sticker.Id }, sticker);
        }

        // POST: Stickers/Delete/5
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var sticker = await _context.StickersDb.FindAsync(id);
            _context.StickersDb.Remove(sticker);
            await _context.SaveChangesAsync();
            return new JsonResult(new { message = "Удаление произошло успешно!" }) { StatusCode = StatusCodes.Status200OK };
        }
        
        private bool StickerExists(int id)
        {
            return _context.StickersDb.Any(e => e.Id == id);
        }

        private bool CategoryExists(int id)
        {
            return _context.CategoriesDb.Any(e => e.Id == id);
        }
    }
}
