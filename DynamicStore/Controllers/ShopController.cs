using DynamicStore.Models;
using DynamicStore.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynamicStore.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;
        public ShopController(AppDbContext context)
        {
            _context = context;
        }
        // اکشن نمایش لیست محصولات و فیلتر بر اساس دسته‌بندی
        // GET: /Shop
        public async Task<IActionResult> Index(
            string? search,
            int? categoryId,
            decimal? minPrice,
            decimal? maxPrice,
            string? sortBy = "newest")
        {
            // ۱. کوئری پایه برای محصولات فعال
            var query = _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .AsQueryable();

            // ۲. فیلتر کلمه کلیدی
            if (!string.IsNullOrWhiteSpace(search))
            {
                var trimmedSearch = search.Trim();
                query = query.Where(p => p.Title.Contains(trimmedSearch) ||
                                         (p.Description != null && p.Description.Contains(trimmedSearch)));
            }

            // ۳. فیلتر دسته‌بندی
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            // ۴. فیلتر بازه قیمت
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            // ۵. مرتب‌سازی
            query = sortBy switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderByDescending(p => p.Id) // پیش‌فرض: جدیدترین
            };

            // ۶. آماده‌سازی خروجی در ViewModel
            var viewModel = new ShopIndexViewModel
            {
                Products = await query.ToListAsync(),
                Categories = await _context.Categories.ToListAsync(),
                Search = search,
                CategoryId = categoryId,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                SortBy = sortBy
            };

            return View(viewModel);
        }


        // GET: /Shop/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.AttributeValues)
                    .ThenInclude(pav => pav.CategoryAttribute)
                .FirstOrDefaultAsync(m => m.Id == id && m.IsActive);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

    }
}
