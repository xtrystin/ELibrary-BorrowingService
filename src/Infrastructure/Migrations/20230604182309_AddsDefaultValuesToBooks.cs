using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELibrary_BorrowingService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddsDefaultValuesToBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PenaltyAmount",
                schema: "borrowingService",
                table: "Book",
                type: "numeric",
                nullable: false,
                defaultValue: 0.1m,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<int>(
                name: "MaxBorrowDays",
                schema: "borrowingService",
                table: "Book",
                type: "integer",
                nullable: false,
                defaultValue: 30,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "MaxBookingDays",
                schema: "borrowingService",
                table: "Book",
                type: "integer",
                nullable: false,
                defaultValue: 15,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PenaltyAmount",
                schema: "borrowingService",
                table: "Book",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldDefaultValue: 0.1m);

            migrationBuilder.AlterColumn<int>(
                name: "MaxBorrowDays",
                schema: "borrowingService",
                table: "Book",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 30);

            migrationBuilder.AlterColumn<int>(
                name: "MaxBookingDays",
                schema: "borrowingService",
                table: "Book",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 15);
        }
    }
}
