using Microsoft.AspNetCore.Http;

namespace Application.Dtos.Products;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public int CategoryId { get; set; }

  
}