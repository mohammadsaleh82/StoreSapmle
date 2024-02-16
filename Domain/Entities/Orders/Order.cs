using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Users;

namespace Domain.Entities.Orders;

public class Order : BaseEntity
{
    public string CustomerId { get; set; } // Foreign key for User
    [ForeignKey(nameof(CustomerId))]
    public User User { get; set; } // Navigation property for User
    public DateTime OrderDate { get; set; } // Order date
    public decimal TotalAmount { get; set; } // Order total amount
    public ICollection<OrderDetail> OrderDetails { get; set; } // Navigation property for OrderDetails
    public ICollection<Payment> Payments { get; set; }
}