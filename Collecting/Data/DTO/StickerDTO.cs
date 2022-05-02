namespace Collecting.Data.DTO
{
    public class StickerDTO
    {
        // id этикетки
        public int Id { set; get; }
        // Фирма-производитель этикетки
        public string Firm { set; get; }
        // Год выпуска этикетки
        public int Year { set; get; }
        // Страна этикетки
        public string Country { set; get; }
        // Материал этикетки
        public string Material { set; get; }
        // Ширина этикетки
        public int Width { set; get; }
        // Высота этикетки
        public int Height { set; get; }
        // Текст этикетки
        public int Text { set; get; }
        // Количество дубликатов
        public int Quantity { set; get; }
        // Стоимость этикетки
        public ushort Price { set; get; }
        // Форма этикетки
        public string Form { set; get; }
        // Ссылка на изображение
        public string Img { set; get; }
        // Ссылка на изображение
        public string AdditionalImg { set; get; }
        // id категории
        public int categoryID { set; get; }
    }
}
