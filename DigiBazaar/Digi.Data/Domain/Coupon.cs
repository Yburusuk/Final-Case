using Digi.Base.Entity;

namespace Digi.Data.Domain;

public class Coupon : BaseEntity
{
    public string CouponCode { get; set; }
    public decimal CouponAmount { get; set; }
    public DateTime ExpireDate { get; set; }
}