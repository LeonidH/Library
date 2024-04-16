using Library.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Library.Data.EntityConfigurations;

public class BookRequestConfiguration : IEntityTypeConfiguration<Entities.BookRequest>
{
    public void Configure(EntityTypeBuilder<BookRequest> builder)
    {
        builder.ToTable("BookRequests");

        builder.HasKey(e => e.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(e => e.Isbn).IsRequired();

        builder.Property(e => e.UserId).IsRequired();

        builder.Property(e => e.RequestType)
            .IsRequired()
            .HasConversion(new EnumToStringConverter<RequestType>())
            .HasMaxLength(6);
        
        builder.Property(e => e.RequestStatus)
            .IsRequired()
            .HasConversion(new EnumToStringConverter<RequestStatus>())
            .HasMaxLength(8);
        
        builder.Property(e => e.RequestedOnUtc).IsRequired();

        builder.Property(e => e.ApprovedOnUtc).IsRequired(false);
        builder.Property(e => e.RejectedOnUtc).IsRequired(false);
        builder.Property(e => e.BorrowedOnUtc).IsRequired(false);
        builder.Property(e => e.ReturnedOnUtc).IsRequired(false);
        
        builder.HasOne(e => e.BookInstance)
            .WithMany()
            .IsRequired()
            .HasForeignKey(e => e.Id)
            .HasConstraintName("FK_BookRequest_BookInstance")
            .OnDelete(DeleteBehavior.Cascade);


        builder.HasOne(e => e.User)
            .WithMany()
            .IsRequired()
            .HasForeignKey(e => e.UserId)
            .HasConstraintName("FK_BookRequest_User")
            .OnDelete(DeleteBehavior.Cascade);
    }
}