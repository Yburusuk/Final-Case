using Digi.Base.Entity;

namespace Digi.Data.Domain;

public class ProductOrderDetail : BaseEntity
{
    public long OrderDetailId { get; set; }
    public long ProductId { get; set; }
    
    public virtual Product Product { get; set; }
    public virtual OrderDetail OrderDetail { get; set; }
}