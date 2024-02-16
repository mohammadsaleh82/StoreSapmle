using Domain.Entities.Products;

namespace Domain.Entities.Orders;

public class OrderDetail : BaseEntity
{
    public int OrderId { get; set; } // Foreign key for Order
    public Order Order { get; set; } // Navigation property for Order
    public ICollection<OrderDetailProduct> DetailProducts { get; set; } // Navigation property for Products
    public decimal TotalPrice { get; set; } // Order total price
    public decimal Discount { get; set; } // Order discount
}