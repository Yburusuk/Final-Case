namespace Digi.Schema;

public class OrderDetailRequest
{
    public string UserName { get; set; }
    public long ProductId { get; set; }
}
public class OrderDetailResponse
{
    public string UserName { get; set; }
    public string OrderNumber { get; set; }
    public List<ProductDetail> Products { get; set; }
}

public class ProductDetail
{
    public string ProductName { get; set; }
    public int Quantity { get; set; }
}