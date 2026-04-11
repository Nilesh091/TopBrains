using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RoleAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("7d5c4e8a-1234-4b5c-8f2e-9a8b7c6d5e4f"), "a1b2c3d4-e5f6-4g7h-8i9j-0k1l2m3n4o5p", "Seller who can list and manage products", "Seller", "SELLER" },
                    { new Guid("8e6d5f9b-2345-4c6d-9a3f-0b9c8d7e6f5d"), "b2c3d4e5-f6g7-4h8i-9j0k-1l2m3n4o5p6q", "Buyer who can add items to cart and place orders", "Buyer", "BUYER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("7d5c4e8a-1234-4b5c-8f2e-9a8b7c6d5e4f"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("8e6d5f9b-2345-4c6d-9a3f-0b9c8d7e6f5d"));
        }
    }
}
