namespace Digi.Schema;

public class OrderRequest
{
    public string UserName { get; set; }
    public string CouponCode { get; set; } = "";
    public bool UseWalletBalance { get; set; } = true;
    
    public string NameSurname { get; set; }
    public string CardNumber { get; set; }
    public int Cvv { get; set; }
    public int ExpirationYear { get; set; }
    public int ExpirationMonth { get; set; }
}

public class OrderResponse
{
    public long Id { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string OrderNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public string CouponCode { get; set; }
    public decimal CouponAmount { get; set; }
    public decimal PointsSpent { get; set; }
    public decimal PointsEarned { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
    
    public List<ProductDetail> Products {get; set; }
}