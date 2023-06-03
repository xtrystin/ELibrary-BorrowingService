using ELibrary_BorrowingService.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibrary_BorrowingService.Infrastructure.EF.Config;

internal class BookEntityTypeConfig : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id);

        builder.ToTable("Book");

        builder.Property<int>("_availabieBooks")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("AvailabieBooks");

        builder.Property<decimal>("_penaltyAmount")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("PenaltyAmount");

        builder.Property(x => x.MaxBookingDays);

        builder.Property(x => x.MaxBorrowDays);
    }
}