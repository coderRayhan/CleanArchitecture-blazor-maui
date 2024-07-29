using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;
public sealed class LookupDetailsConfiguration
    : IEntityTypeConfiguration<LookupDetails>
{
    public void Configure(EntityTypeBuilder<LookupDetails> builder)
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
