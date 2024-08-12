using Digi.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Digi.Data.Configuration;

public class ProductOrderDetailConfiguration : IEntityTypeConfiguration<ProductOrderDetail>
{
    public void Configure(EntityTypeBuilder<ProductOrderDetail> builder)
    {
        builder.Property(x => x.CreatedDate).IsRequired(true);
        builder.Property(x => x.IsActive).IsRequired(true);
        builder.Property(x => x.ProductId).IsRequired(true);
        builder.Property(x => x.OrderDetailId).IsRequired(true);
    }
}