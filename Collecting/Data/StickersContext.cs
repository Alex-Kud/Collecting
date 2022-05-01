using Collecting.Models;
using Microsoft.EntityFrameworkCore;

namespace Collecting.Data
{
    public class StickersContext : DbContext
    {
        public DbSet<Sticker> StickersDb { get; set; }
        public DbSet<Category> CategoriesDb { get; set; }
        public StickersContext(DbContextOptions<StickersContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }
    }
}
