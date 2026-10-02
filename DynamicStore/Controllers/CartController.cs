using DynamicStore.Extensions;
using DynamicStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace DynamicStore.Controllers
{
    public class CartController : Controller
    {

        private readonly AppDbContext _context;
        private const string CartSessionKey = "DynamicStore_Cart";
        public CartController(AppDbContext context)
        {
            _context = context;
        }


        // نمایش سبد خرید
        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }
        // افزودن محصول به سبد خرید (از صفحه جزئیات یا فروشگاه)
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            if (quantity <= 0) quantity = 1;

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            var cart = GetCart();
            var item = cart.Items.FirstOrDefault(x => x.ProductId == productId);

            if (item != null)
            {
                item.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new CartItemViewModel
                {
                    ProductId = product.Id,
                    Title = product.Title,
                    Price = product.Price,
                    ImageUrl = product.ImageName,
                    Quantity = quantity
                });
            }

            SaveCart(cart);

            TempData["Success"] = $"«{product.Title}» به سبد خرید اضافه شد.";
            return RedirectToAction(nameof(Index));
        }
        // به‌روزرسانی تعداد یک کالا
        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int count)
        {
            var cart = GetCart();
            var item = cart.Items.FirstOrDefault(x => x.ProductId == productId);

            if (item != null)
            {
                if (count > 0)
                {
                    item.Quantity = count;
                }
                else
                {
                    cart.Items.Remove(item);
                }
                SaveCart(cart);
            }

            return RedirectToAction(nameof(Index));
        }
        // حذف یک کالا
        [HttpPost]
        public IActionResult Remove(int productId)
        {
            var cart = GetCart();
            var item = cart.Items.FirstOrDefault(x => x.ProductId == productId);

            if (item != null)
            {
                cart.Items.Remove(item);
                SaveCart(cart);
                TempData["Info"] = "آیتم از سبد خرید حذف شد.";
            }

            return RedirectToAction(nameof(Index));
        }

        // خالی کردن کل سبد خرید
        [HttpPost]
        public IActionResult Clear()
        {
            HttpContext.Session.Remove(CartSessionKey);
            return RedirectToAction(nameof(Index));
        }
        // متدهای کمکی داخلی
        private CartViewModels GetCart()
        {
            return HttpContext.Session.GetObject<CartViewModels>(CartSessionKey) ?? new CartViewModels();
        }
        private void SaveCart(CartViewModels cart)
        {
            HttpContext.Session.SetObject(CartSessionKey, cart);
        }
    }
}
