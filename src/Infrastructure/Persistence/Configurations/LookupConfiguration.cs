using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;
internal sealed class LookupConfiguration :
    IEntityTypeConfiguration<Lookups>
{
    public void Configure(EntityTypeBuilder<Lookups> builder)
    {
        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.HasIndex(x => x.ParentId);

        builder.Property(x => x.Code)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(500);
    }
}
