using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Products;

namespace Domain.Entities.Orders;

public class OrderDetailProduct
{
    public int Id { get; set; }
    public int OrderDetailId { get; set; }
    public int ProductId { get; set; }
    [ForeignKey(nameof(ProductId))] public Product Product { get; set; }

    [ForeignKey(nameof(OrderDetailId))] public OrderDetail OrderDetail { get; set; }
}