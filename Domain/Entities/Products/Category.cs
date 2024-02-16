namespace Domain.Entities.Products;

// Database model for Category
public class Category : BaseEntity
{
    public string Name { get; set; } // Category name
    public int? ParentId { get; set; } // Foreign key for parent category
    public Category Parent { get; set; } // Navigation property for parent category
    public ICollection<Category> Children { get; set; } // Navigation property for sub categories
    public ICollection<Product> Products { get; set; } // Navigation property for Products
}
