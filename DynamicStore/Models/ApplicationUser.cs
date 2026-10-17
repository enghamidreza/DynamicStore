using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DynamicStore.Models
{
    //جداول مربوط ب ساخت یوزر رو خودش در دیتابیس میسازه و روابط امنیتی رو مشخص میکنه :IdentityUser
    public class ApplicationUser: IdentityUser
    {
        //مقدار اولیه رشته خالی :string.Empty
        public string FullName { get; set; } = string.Empty;
        public string ? Address { get; set; }
        public string ? PostalCode { get; set; }
    }
}
