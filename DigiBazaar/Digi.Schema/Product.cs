namespace Digi.Schema;

public class ProductRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int MaxPoints { get; set; }
    public double PointsPercentage { get; set; }
}

public class ProductResponse
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> CategoryNames { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int MaxPoints { get; set; }
    public double PointsPercentage { get; set; }
}