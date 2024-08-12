using Digi.Base.Entity;

namespace Digi.Data.Domain;

public class Product : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int MaxPoints { get; set; }
    public decimal PointsPercentage { get; set; }
    
    public virtual List<Category> Categories { get; set; }
    public virtual List<OrderDetail> OrderDetails { get; set; }
    public virtual List<ProductOrderDetail> ProductOrderDetails { get; set; }
}