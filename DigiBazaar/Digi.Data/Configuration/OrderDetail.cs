using Digi.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Digi.Data.Configuration;

public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.Property(x => x.CreatedDate).IsRequired(true);
        builder.Property(x => x.IsActive).IsRequired(true);
        builder.Property(x => x.UserId).IsRequired(true);
        builder.Property(x => x.UserName).IsRequired(true);
        builder.Property(x => x.OrderId).IsRequired(true);
        
        builder.HasMany(od => od.Products)
            .WithMany(p => p.OrderDetails)
            .UsingEntity<ProductOrderDetail>(
                j => j.HasOne(po => po.Product)
                    .WithMany(p => p.ProductOrderDetails)
                    .HasForeignKey(po => po.ProductId)
                    .OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne(po => po.OrderDetail)
                    .WithMany(od => od.ProductOrderDetails)
                    .HasForeignKey(po => po.OrderDetailId)
                    .OnDelete(DeleteBehavior.Restrict),
                j => j.HasKey(po => po.Id)
            );
    }
}