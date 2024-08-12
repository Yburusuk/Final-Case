using Digi.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Digi.Data.Configuration;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {

        builder.Property(x => x.FirstName).IsRequired(true).HasMaxLength(50);
        builder.Property(x => x.LastName).IsRequired(true).HasMaxLength(50);
        builder.Property(x => x.Role).IsRequired(true).HasMaxLength(6);
        builder.Property(x => x.WalletBalance).IsRequired(true).HasPrecision(10, 2);
        
        builder.HasMany(x => x.Orders)
            .WithOne(x => x.ApplicationUser)
            .HasForeignKey(x => x.UserId)
            .IsRequired(false);
        
        builder.HasMany(x => x.OrderDetails)
            .WithOne(x => x.ApplicationUser)
            .HasForeignKey(x => x.UserId)
            .IsRequired(false);
    }
}