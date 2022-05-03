using System.ComponentModel.DataAnnotations;

namespace Collecting.Data.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        private List<CartItem> _itemsCollection = new();
        public IEnumerable<CartItem> Lines
        {
            get { return _itemsCollection; }
        }
    }
}
