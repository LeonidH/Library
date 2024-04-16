using Library.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Library.Data.EntityConfigurations;

public class BookInstanceConfiguration : IEntityTypeConfiguration<BookInstance>
{
    public void Configure(EntityTypeBuilder<BookInstance> builder)
    {
        builder.ToTable("BookInstances");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Isbn).IsRequired();

        builder.Property(e => e.BookId).IsRequired();

        builder.Property(e => e.LibraryId).IsRequired();
        
        builder.Property(e => e.Status)
            .IsRequired()
            .HasConversion(new EnumToStringConverter<BookInstanceStatus>())
            .HasMaxLength(10);
        
        builder.HasOne(e => e.Library)
            .WithMany(e => e.BookInstances)
            .HasForeignKey(e => e.LibraryId)
            .HasConstraintName("FK_BookInstance_Library")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Book)
            .WithMany()
            .IsRequired()
            .HasForeignKey(e => e.BookId)
            .HasConstraintName("FK_BookInstance_Book")
            .OnDelete(DeleteBehavior.NoAction);
    }
}