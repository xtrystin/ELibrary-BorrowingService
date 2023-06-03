using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ELibrary_BorrowingService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class iniCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "borrowingService");

            migrationBuilder.CreateTable(
                name: "Book",
                schema: "borrowingService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaxBorrowDays = table.Column<int>(type: "integer", nullable: false),
                    MaxBookingDays = table.Column<int>(type: "integer", nullable: false),
                    AvailabieBooks = table.Column<int>(type: "integer", nullable: false),
                    PenaltyAmount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                schema: "borrowingService",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CurrentBookedBookNr = table.Column<int>(type: "integer", nullable: false),
                    CurrentBorrowedBookNr = table.Column<int>(type: "integer", nullable: false),
                    IsAccountBlocked = table.Column<bool>(type: "boolean", nullable: false),
                    MaxBooksToBook = table.Column<int>(type: "integer", nullable: false),
                    MaxBooksToBorrow = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookingHistory",
                schema: "borrowingService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BookId = table.Column<int>(type: "integer", nullable: false),
                    CustomerId = table.Column<string>(type: "text", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BookingLimitDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsSuccessful = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingHistory_Book_BookId",
                        column: x => x.BookId,
                        principalSchema: "borrowingService",
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingHistory_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "borrowingService",
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BorrowingHistory",
                schema: "borrowingService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BookId = table.Column<int>(type: "integer", nullable: false),
                    CustomerId = table.Column<string>(type: "text", nullable: false),
                    BorrowedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReturnedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowingHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BorrowingHistory_Book_BookId",
                        column: x => x.BookId,
                        principalSchema: "borrowingService",
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BorrowingHistory_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "borrowingService",
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingHistory_BookId",
                schema: "borrowingService",
                table: "BookingHistory",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingHistory_CustomerId",
                schema: "borrowingService",
                table: "BookingHistory",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowingHistory_BookId",
                schema: "borrowingService",
                table: "BorrowingHistory",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowingHistory_CustomerId",
                schema: "borrowingService",
                table: "BorrowingHistory",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingHistory",
                schema: "borrowingService");

            migrationBuilder.DropTable(
                name: "BorrowingHistory",
                schema: "borrowingService");

            migrationBuilder.DropTable(
                name: "Book",
                schema: "borrowingService");

            migrationBuilder.DropTable(
                name: "Customer",
                schema: "borrowingService");
        }
    }
}
