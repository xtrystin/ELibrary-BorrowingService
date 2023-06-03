using ELibrary_BorrowingService.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibrary_BorrowingService.Infrastructure.EF.Config;
internal class CustomerTypeConfig : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customer");
        builder.HasKey(x => x.Id);

        builder.Property<bool>("_isAccountBlocked")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("IsAccountBlocked");

        builder.Property<int>("_currentBookedBookNr")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("CurrentBookedBookNr");

        builder.Property<int>("_currentBorrowedBookNr")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("CurrentBorrowedBookNr");

        builder.Property<int>("_maxBooksToBook")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("MaxBooksToBook");

        builder.Property<int>("_maxBooksToBorrow")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("MaxBooksToBorrow");
    }
}
