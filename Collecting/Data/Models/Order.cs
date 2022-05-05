namespace Collecting.Data.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
    }
}
