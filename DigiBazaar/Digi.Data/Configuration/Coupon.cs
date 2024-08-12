using Digi.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Digi.Data.Configuration;

public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.Property(x => x.CreatedDate).IsRequired(true);
        builder.Property(x => x.IsActive).IsRequired(true);
        builder.Property(x => x.CouponCode).IsRequired(true).HasMaxLength(10);
        builder.Property(x => x.CouponAmount).IsRequired(true).HasPrecision(10, 2);
        builder.Property(x => x.ExpireDate).IsRequired(true);
        
        builder.HasIndex(x => new { x.CouponCode }).IsUnique(true);
    }
}