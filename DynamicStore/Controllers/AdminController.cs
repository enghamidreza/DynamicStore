using DynamicStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

//پنل ادمین
[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.TotalProducts = await _context.Products.CountAsync();
        ViewBag.TotalCategories = await _context.Categories.CountAsync();
         ViewBag.TotalOrders = await _context.Orders.CountAsync();

        // محاسبه مجموع درآمدهای پرداخت‌شده
        ViewBag.TotalRevenue = (await _context.Orders
            .Where(o => o.IsPaid)
            .SumAsync(o => (decimal?)o.TotalAmount)) ?? 0m;

        // بارگذاری آخرین ۵ سفارش 
        ViewBag.RecentOrders = await _context.Orders
            .AsNoTracking()
            .OrderByDescending(o => o.OrderDate)
            //معادل 5 تا محصول
            .Take(5)
            //
            .ToListAsync();
        return View();
    }
}
