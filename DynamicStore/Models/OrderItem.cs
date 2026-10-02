using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicStore.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; } = null!;

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; } = null!;

        public decimal Price { get; set; } // قیمت در زمان خرید ثبت می‌شود تا با تغییر قیمت محصول در آینده، فاکتور تغییر نکند
        public int Quantity { get; set; }
    }
}
