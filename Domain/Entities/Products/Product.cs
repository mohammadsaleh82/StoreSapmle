using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Orders;

namespace Domain.Entities.Products;

public class Product : BaseEntity
{
    public string Name { get; set; } // Product name
    public decimal Price { get; set; } // Product price
    public string Description { get; set; } // Product description
    public string Image { get; set; }
    public int CategoryId { get; set; } // Foreign key for Category
    [ForeignKey(nameof(CategoryId))] public Category Category { get; set; } // Navigation property for Category

    public ICollection<OrderDetailProduct> DetailProducts { get; set; }
}