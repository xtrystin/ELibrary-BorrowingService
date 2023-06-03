using ELibrary_BorrowingService.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibrary_BorrowingService.Infrastructure.EF.Config;
public class BookingHistoryTypeConfig : IEntityTypeConfiguration<BookingHistory>
{
    public void Configure(EntityTypeBuilder<BookingHistory> builder)
    {
        builder.ToTable("BookingHistory");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property<DateTime>("_bookingDate")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("BookingDate");

        builder.Property<DateTime>("_bookingLimitDate")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("BookingLimitDate");

        builder.Property<bool>("_isActive")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("IsActive");

        builder.Property<bool?>("_isSuccessful")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .IsRequired(false)
            .HasColumnName("IsSuccessful");
    }
}
