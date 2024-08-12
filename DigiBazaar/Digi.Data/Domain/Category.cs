using Digi.Base.Entity;

namespace Digi.Data.Domain;

public class Category : BaseEntity
{
    public string Name { get; set; }
    
    public virtual List<Product> Products { get; set; }
}