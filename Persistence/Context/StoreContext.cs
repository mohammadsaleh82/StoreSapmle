using Domain.Entities.Orders;
using Domain.Entities.Products;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context;

public class StoreContext : IdentityDbContext<User>
{
    public StoreContext(DbContextOptions<StoreContext> options) : base(options)
    {

    }

    #region Dbsets

    public DbSet<OrderDetailProduct> DetailProducts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<Payment> Payments { get; set; }

    #endregion


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);
        builder.Entity<User>().HasQueryFilter(c => !c.IsDeleted);
        builder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
        builder.Entity<Order>().HasQueryFilter(o => !o.IsDeleted);
        builder.Entity<OrderDetail>().HasQueryFilter(od => !od.IsDeleted);
        builder.Entity<Payment>().HasQueryFilter(p => !p.IsDeleted);

    }
}