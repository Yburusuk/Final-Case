using Digi.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Digi.Data.Configuration;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(x => x.CreatedDate).IsRequired(true);
        builder.Property(x => x.IsActive).IsRequired(true);
        builder.Property(x => x.UserId).IsRequired(true);
        builder.Property(x => x.UserName).IsRequired(true);
        builder.Property(x => x.OrderNumber).IsRequired(true).HasMaxLength(9);
        builder.Property(x => x.TotalAmount).IsRequired(true).HasPrecision(10, 2);
        builder.Property(x => x.CouponCode).IsRequired(false).HasMaxLength(10);
        builder.Property(x => x.CouponAmount).IsRequired(true).HasPrecision(10, 2);
        builder.Property(x => x.PointsSpent).IsRequired(true).HasPrecision(10, 2);
        builder.Property(x => x.PointsEarned).IsRequired(true).HasPrecision(10, 2);

        builder.HasIndex(x => new { x.OrderNumber }).IsUnique(true);
        
        builder.HasOne(x => x.OrderDetail)
            .WithOne(x => x.Order)
            .HasForeignKey<OrderDetail>(x => x.OrderId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Cascade);
    }
}