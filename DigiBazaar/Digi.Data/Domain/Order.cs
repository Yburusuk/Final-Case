using Digi.Base.Entity;

namespace Digi.Data.Domain;

public class Order : BaseEntity
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string OrderNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public string CouponCode { get; set; }
    public decimal CouponAmount { get; set; }
    public decimal PointsSpent { get; set; }
    public decimal PointsEarned { get; set; }
    
    public virtual ApplicationUser ApplicationUser {get; set; }
    public virtual OrderDetail OrderDetail {get; set; }
}