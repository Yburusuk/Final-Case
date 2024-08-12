namespace Digi.Schema;

public class CouponRequest
{
    public string CouponCode { get; set; }
    public decimal CouponAmount { get; set; }
    public DateTime ExpireDate { get; set; }
}

public class GenerateCouponRequest
{
    public decimal CouponAmount { get; set; }
    public DateTime ExpireDate { get; set; }
}

public class CouponResponse
{
    public long Id { get; set; }
    public string CouponCode { get; set; }
    public decimal CouponAmount { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ExpireDate { get; set; }
    public bool IsActive { get; set; }
}