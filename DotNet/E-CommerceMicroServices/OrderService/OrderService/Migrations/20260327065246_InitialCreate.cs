using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OrderService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "OrderDate", "ProductName", "Quantity", "TotalPrice", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 6, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), "Laptop", 1, 1200.99m, 1 },
                    { 2, new DateTime(2024, 6, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), "Mouse", 2, 40.50m, 1 },
                    { 3, new DateTime(2024, 6, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), "Keyboard", 1, 70.00m, 2 },
                    { 4, new DateTime(2024, 6, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), "Monitor", 1, 250.00m, 2 },
                    { 5, new DateTime(2024, 6, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), "USB Cable", 3, 15.75m, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
