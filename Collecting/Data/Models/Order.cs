namespace Collecting.Data.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
        public OrderStatus Status { get; set; }
    }
}
