using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Data.EntityConfigurations;

public class LibraryConfiguration : IEntityTypeConfiguration<Entities.Library>
{
    public void Configure(EntityTypeBuilder<Entities.Library> builder)
    {
        builder.ToTable("Libraries");

        builder.HasKey(e => e.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(e => e.Name).IsRequired().HasMaxLength(128);

        builder.Property(e => e.Address).IsRequired().HasMaxLength(256);

        builder.Property(e => e.City).IsRequired().HasMaxLength(16);

        builder.Property(e => e.Country).IsRequired().HasMaxLength(16);

        builder.Property(e => e.PostalCode).IsRequired().HasMaxLength(16);

        builder.HasMany(e => e.Members)
            .WithMany();
    }
}