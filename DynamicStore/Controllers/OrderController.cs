using DynamicStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DynamicStore.Extensions;

namespace DynamicStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        // تعریف دقیق همان کلیدی که در CartController استفاده شده است
        private const string CartSessionKey = "DynamicStore_Cart";

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        // نمایش فرم دریافت اطلاعات مشتری
        [HttpGet]
        public IActionResult Checkout()
        {
            // خواندن دقیق مدل CartViewModels با همان کلید اصلی
            var cart = HttpContext.Session.GetObject<CartViewModels>(CartSessionKey) ?? new CartViewModels();

            if (cart.Items == null || cart.Items.Count == 0)
            {
                return RedirectToAction("Index", "Cart"); // اگر سبد خالی بود برگردد به سبد
            }
            return View();
        }

        // پردازش فرم و ذخیره در دیتابیس
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var cart = HttpContext.Session.GetObject<CartViewModels>(CartSessionKey) ?? new CartViewModels();

            if (cart.Items == null || cart.Items.Count == 0)
            {
                return RedirectToAction("Index", "Cart");
            }

            // ۱. ثبت اطلاعات کلی سفارش
            var order = new Order
            {
                CustomerName = model.CustomerName,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                OrderDate = DateTime.Now,
                // محاسبه مجموع از روی لیست Items
                TotalAmount = cart.Items.Sum(c => c.Price * c.Quantity),
                IsPaid = false // در پروژه واقعی اینجا به درگاه بانکی وصل می‌شود
            };

            _context.Orders.Add(order);
            _context.SaveChanges(); // ذخیره میکنیم تا Id سفارش ساخته شود

            // ۲. ثبت تک تک کالاهای داخل سبد خرید برای این سفارش
            foreach (var item in cart.Items)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Price = item.Price,
                    Quantity = item.Quantity
                };
                _context.OrderItems.Add(orderItem);
            }
            _context.SaveChanges();

            // ۳. پاک کردن سبد خرید (چون خرید موفق بود)
            HttpContext.Session.Remove(CartSessionKey);

            // ۴. انتقال کاربر به صفحه تایید نهایی
            return RedirectToAction("Success", new { orderId = order.Id });
        }

        // صفحه موفقیت‌آمیز بودن ثبت سفارش
        public IActionResult Success(int orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }
    }
}
