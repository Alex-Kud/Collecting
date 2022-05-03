using System.Text.Json.Serialization;

namespace Collecting.Data.Models
{
    public class Category
    {
        // id категории
        public int Id { set; get; }
        // Название категории
        public string Name { set; get; }
        // Описание категории
        public string Description { set; get; }
        // Список товаров в этой категории
        [JsonIgnore]
        public List<Sticker> Stickers { set; get; } = new List<Sticker>();
    }
}
