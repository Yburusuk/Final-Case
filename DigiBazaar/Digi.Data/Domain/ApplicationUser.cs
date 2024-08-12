using Microsoft.AspNetCore.Identity;

namespace Digi.Data.Domain;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Role { get; set; }
    public decimal WalletBalance { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
    
    public virtual List<Order> Orders {get; set;}
    public virtual List<OrderDetail> OrderDetails {get; set;}
}