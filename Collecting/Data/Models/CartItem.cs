namespace Collecting.Data.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice
        {
            get { return Sticker.Price; }
        }
        public decimal TotalPrice
        {
            get { return UnitPrice * Quantity; }
        }
        public int StickerId { get; set; }
        public Sticker Sticker { get; set; }
        public int CartId { get; set; }
    }
}
