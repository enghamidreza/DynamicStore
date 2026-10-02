using System.ComponentModel.DataAnnotations;

namespace DynamicStore.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "نام و نام خانوادگی الزامی است")]
        [MaxLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "شماره تماس الزامی است")]
        [MaxLength(15)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "آدرس کامل الزامی است")]
        public string Address { get; set; } = string.Empty;

        public decimal TotalAmount { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public bool IsPaid { get; set; } = false; // وضعیت پرداخت

        // ارتباط با اقلام سفارش
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
