using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.Migrations
{
    /// <inheritdoc />
    public partial class dbupdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyProperty_Consumers_CartId",
                table: "MyProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_MyProperty_Orders_ItemId",
                table: "MyProperty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MyProperty",
                table: "MyProperty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Consumers",
                table: "Consumers");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Items");

            migrationBuilder.RenameTable(
                name: "MyProperty",
                newName: "OrderItems");

            migrationBuilder.RenameTable(
                name: "Consumers",
                newName: "Carts");

            migrationBuilder.RenameIndex(
                name: "IX_MyProperty_ItemId",
                table: "OrderItems",
                newName: "IX_OrderItems_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_MyProperty_CartId",
                table: "OrderItems",
                newName: "IX_OrderItems_CartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Items",
                table: "Items",
                column: "ItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Carts",
                table: "Carts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Carts_CartId",
                table: "OrderItems",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Items_ItemId",
                table: "OrderItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Carts_CartId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Items_ItemId",
                table: "OrderItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Items",
                table: "Items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Carts",
                table: "Carts");

            migrationBuilder.RenameTable(
                name: "OrderItems",
                newName: "MyProperty");

            migrationBuilder.RenameTable(
                name: "Items",
                newName: "Orders");

            migrationBuilder.RenameTable(
                name: "Carts",
                newName: "Consumers");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_ItemId",
                table: "MyProperty",
                newName: "IX_MyProperty_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_CartId",
                table: "MyProperty",
                newName: "IX_MyProperty_CartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyProperty",
                table: "MyProperty",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "ItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Consumers",
                table: "Consumers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MyProperty_Consumers_CartId",
                table: "MyProperty",
                column: "CartId",
                principalTable: "Consumers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MyProperty_Orders_ItemId",
                table: "MyProperty",
                column: "ItemId",
                principalTable: "Orders",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
