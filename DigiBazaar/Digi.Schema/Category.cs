namespace Digi.Schema;

public class CategoryRequest
{
    public string Name { get; set; }
}

public class CategoryResponse
{
    public long Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
}