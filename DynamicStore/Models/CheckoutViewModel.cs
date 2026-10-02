using System.ComponentModel.DataAnnotations;

namespace DynamicStore.Models
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "لطفاً نام خود را وارد کنید.")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "لطفاً شماره موبایل را وارد کنید.")]
        [RegularExpression(@"^09\d{9}$", ErrorMessage = "شماره موبایل معتبر نیست (مثال: 09123456789)")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "لطفاً آدرس دقیق پستی را وارد کنید.")]
        public string Address { get; set; } = string.Empty;
    }
}
