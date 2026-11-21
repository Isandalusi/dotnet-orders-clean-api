using Microsoft.EntityFrameworkCore;
using Orders.Domain.Entities;

namespace Orders.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.OrderNumber).IsRequired().HasMaxLength(40);
            entity.HasIndex(o => o.OrderNumber).IsUnique();
            entity.Property(o => o.CustomerName).IsRequired().HasMaxLength(120);
            entity.Property(o => o.TotalAmount).HasPrecision(18, 2);
            entity.Property(o => o.Status).HasConversion<string>().HasMaxLength(30);
        });
    }
}
