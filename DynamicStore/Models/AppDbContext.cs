using Microsoft.EntityFrameworkCore;

namespace DynamicStore.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryAttribute> CategoryAttributes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductAttributeValue> ProductAttributeValues { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // جلوگیری از حذف آبشاری (Cascade Delete) در مقادیر ویژگی‌ها تا خطای سیکل در SQL Server نگیریم
            modelBuilder.Entity<ProductAttributeValue>()
                .HasOne(p => p.CategoryAttribute)
                .WithMany()
                .HasForeignKey(p => p.CategoryAttributeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
