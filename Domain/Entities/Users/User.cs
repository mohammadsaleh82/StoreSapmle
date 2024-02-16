using System.ComponentModel.DataAnnotations;
using Domain.Entities.Orders;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Users;

public class User : IdentityUser
{
    [MaxLength(50)]
    public string Avatar { get; set; }
    public bool IsDeleted { get; set; } = false;
    public ICollection<Order> Orders { get; set; }
}