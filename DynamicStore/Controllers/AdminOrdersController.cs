using DynamicStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynamicStore.Controllers
{
    // این کنترلر باید بعداً با Authorize محدود شود
     [Authorize(Roles = "Admin")] 
    public class AdminOrdersController : Controller
    {
        private readonly AppDbContext _context;

        public AdminOrdersController(AppDbContext context)
        {
            _context = context;
        }
        // GET: /AdminOrders
        // نمایش لیست کلیه سفارشات
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                                       .AsNoTracking()
                                       .OrderByDescending(o => o.OrderDate)
                                       .ToListAsync();

            return View(orders);
        }
        // GET: /AdminOrders/Details/5
        // نمایش جزئیات کامل یک سفارش خاص
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.OrderItems) // لود کردن آیتم‌های سفارش
                    .ThenInclude(oi => oi.Product) // لود کردن اطلاعات محصول برای هر آیتم
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
