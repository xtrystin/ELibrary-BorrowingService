using ELibrary_BorrowingService.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibrary_BorrowingService.Infrastructure.EF.Config;
public class BorrowingHistoryTypeConfig : IEntityTypeConfiguration<BorrowingHistory>
{
    public void Configure(EntityTypeBuilder<BorrowingHistory> builder)
    {
        builder.ToTable("BorrowingHistory");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property<DateTime>("_borrowedDate")
           .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("BorrowedDate");

        builder.Property<DateTime?>("_returnedDate")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .IsRequired(false)
            .HasColumnName("ReturnedDate");
    }
}
