namespace InventoryWebApp.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        // 自動計算プロパティ
        public decimal TotalValue => Quantity * Price;
    }
}
