using Collecting.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Collecting.Data
{
    public class StickersContext : DbContext
    {
        public DbSet<Sticker> StickersDb { get; set; }
        public DbSet<Category> CategoriesDb { get; set; }
        public DbSet<User> UsersDb { get; set; }
        public DbSet<CartItem> CartItemsDb { get; set; }
        public DbSet<Cart> CartsDb { get; set; }
        public StickersContext(DbContextOptions<StickersContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }
    }
}
