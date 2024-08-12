using Digi.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Digi.Data.Configuration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(x => x.CreatedDate).IsRequired(true);
        builder.Property(x => x.IsActive).IsRequired(true);
        builder.Property(x => x.Name).IsRequired(true).HasMaxLength(50);
        builder.Property(x => x.Description).IsRequired(true).HasMaxLength(100);
        builder.Property(x => x.Price).IsRequired(true).HasPrecision(10, 2);
        builder.Property(x => x.Stock).IsRequired(true);
        builder.Property(x => x.MaxPoints).IsRequired(true);
        builder.Property(x => x.PointsPercentage).IsRequired(true).HasPrecision(3, 2);

        builder.HasIndex(x => new { x.Name }).IsUnique(true);
        
        builder.HasMany(p => p.Categories)
            .WithMany(c => c.Products)
            .UsingEntity<Dictionary<string, object>>(
                "ProductCategory",
                j => j.HasOne<Category>()
                    .WithMany()
                    .HasForeignKey("CategoryId")
                    .OnDelete(DeleteBehavior.Restrict),
                j => j.HasOne<Product>()
                    .WithMany()
                    .HasForeignKey("ProductId")
                    .OnDelete(DeleteBehavior.Cascade)
            );
        
        builder.HasMany(p => p.OrderDetails)
            .WithMany(od => od.Products)
            .UsingEntity<ProductOrderDetail>(
                j => j.HasOne(po => po.OrderDetail)
                    .WithMany(od => od.ProductOrderDetails)
                    .HasForeignKey(po => po.OrderDetailId)
                    .OnDelete(DeleteBehavior.Restrict),
                j => j.HasOne(po => po.Product)
                    .WithMany(p => p.ProductOrderDetails)
                    .HasForeignKey(po => po.ProductId)
                    .OnDelete(DeleteBehavior.Cascade),
                j => j.HasKey(po => po.Id)
            );
    }
}