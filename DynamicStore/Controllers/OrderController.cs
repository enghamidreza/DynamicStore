using DynamicStore.Extensions; // برای GetObject روی Session
using DynamicStore.Models;
using DynamicStore.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicStore.Controllers
{
    [Authorize] // کل فرایند ثبت سفارش فقط برای کاربران واردشده مجاز است
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private const string CartSessionKey = "DynamicStore_Cart";

        public OrderController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // نمایش فرم دریافت اطلاعات مشتری
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var cart = HttpContext.Session.GetObject<CartViewModels>(CartSessionKey) ?? new CartViewModels();

            if (cart.Items == null || cart.Items.Count == 0)
            {
                return RedirectToAction("Index", "Cart");
            }

            // خواندن کاربر فعلی برای پر کردن خودکار فیلدهای فرم
            var user = await _userManager.GetUserAsync(User);

            var model = new CheckoutViewModel
            {
                CustomerName = user?.FullName ?? string.Empty,
                PhoneNumber = user?.PhoneNumber ?? string.Empty,
                Address = user?.Address ?? string.Empty
            };

            return View(model);
        }

        // پردازش فرم و ذخیره در دیتابیس
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            var cart = HttpContext.Session.GetObject<CartViewModels>(CartSessionKey) ?? new CartViewModels();

            if (cart.Items == null || cart.Items.Count == 0)
            {
                return RedirectToAction("Index", "Cart");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // استخراج کاربر فعلی جهت انتساب به سفارش
            var user = await _userManager.GetUserAsync(User);

            // ۱. ثبت اطلاعات کلی سفارش
            var order = new Order
            {
                UserId = user?.Id, // وصل کردن سفارش به آیدی کاربر
                CustomerName = model.CustomerName,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                OrderDate = DateTime.Now,
                TotalAmount = cart.Items.Sum(c => c.Price * c.Quantity),
                IsPaid = false // درگاه پرداخت
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(); // ایجاد Order.Id

            // ۲. ثبت اقلام داخل سبد خرید
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
            await _context.SaveChangesAsync();

            // ۳. پاک کردن سبد خرید بعد از ثبت موفق
            HttpContext.Session.Remove(CartSessionKey);

            // ۴. انتقال به صفحه تایید نهایی
            return RedirectToAction("Success", new { orderId = order.Id });
        }

        // صفحه موفقیت‌آمیز بودن ثبت سفارش
        [HttpGet]
        public IActionResult Success(int orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }
    }
}
