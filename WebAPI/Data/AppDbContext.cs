using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Data
{
    public class AppDbContext : DbContext
    {
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<WebAPI.Models.MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuItem>(entity =>
            {
                entity.ToTable("menu_items");

                entity.Property(m => m.Id).HasColumnName("id");
                entity.Property(m => m.Name).HasColumnName("name");
                entity.Property(m => m.Price).HasColumnName("price").HasPrecision(10, 2);
                entity.Property(m => m.Category).HasColumnName("category");
                entity.Property(m => m.IsAvailable).HasColumnName("is_available"); 
            });


            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");
                entity.Property(o => o.Id).HasColumnName("id");
                entity.Property(o => o.TotalAmount).HasColumnName("total_amount").HasPrecision(12, 2);
                entity.Property(o => o.CreatedAt).HasColumnName("created_at");
            });

 
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("order_items");
                entity.Property(oi => oi.Id).HasColumnName("id");
                entity.Property(oi => oi.OrderId).HasColumnName("order_id");
                entity.Property(oi => oi.MenuItemId).HasColumnName("menu_item_id");
                entity.Property(oi => oi.Price).HasColumnName("price").HasPrecision(10, 2);
                entity.Property(oi => oi.Quantity).HasColumnName("quantity");

                //entity.HasOne(oi => oi.Order)
                //      .WithMany(o => o.OrderItems)
                //      .HasForeignKey(oi => oi.OrderId)
                //      .OnDelete(DeleteBehavior.Cascade);

                //entity.HasOne(oi => oi.MenuItem)
                //      .WithMany()
                //      .HasForeignKey(oi => oi.MenuItemId)
                //      .OnDelete(DeleteBehavior.Restrict);
            });
        }


    }
}
