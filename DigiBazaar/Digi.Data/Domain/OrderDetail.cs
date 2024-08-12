using Digi.Base.Entity;

namespace Digi.Data.Domain;

public class OrderDetail : BaseEntity
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public long OrderId { get; set; }
    
    public virtual ApplicationUser ApplicationUser { get; set; }
    public virtual Order Order { get; set; }
    public virtual List<Product> Products { get; set; }
    public virtual List<ProductOrderDetail> ProductOrderDetails { get; set; }
}