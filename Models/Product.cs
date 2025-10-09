using System.ComponentModel.DataAnnotations;

namespace InventoryWebApp.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "商品名は必須です。")]
        public string Name { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "数量は0以上で入力してください。")]
        public int Quantity { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "価格は1以上で入力してください。")]
        public int Price { get; set; }

        // 自動計算プロパティ
        public int TotalValue => Quantity * Price;
    }
}
