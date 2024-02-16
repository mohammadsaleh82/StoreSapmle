using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Orders;
 
public class Payment:BaseEntity
{
    // This property holds the code of the payment
    public string Code { get; set; }
    // This property indicates whether the payment was successful or not
    public bool IsSuccessed { get; set; }
    // This property holds the ID of the order associated with the payment
    public int  OrderId { get; set; }
    // This attribute specifies the foreign key relationship between the payment and the order tables
    [ForeignKey(nameof(OrderId))]
    // This property holds the order object related to the payment
    public Order  Order { get; set; }
}
