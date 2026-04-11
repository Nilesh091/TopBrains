using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash" },
                values: new object[,]
                {
                    { 1, "alice@example.com", "Alice", "hashedpassword1" },
                    { 2, "bob@example.com", "Bob", "hashedpassword2" },
                    { 3, "charlie@example.com", "Charlie", "hashedpassword3" },
                    { 4, "david@example.com", "David", "hashedpassword4" },
                    { 5, "eve@example.com", "Eve", "hashedpassword5" },
                    { 6, "frank@example.com", "Frank", "hashedpassword6" },
                    { 7, "grace@example.com", "Grace", "hashedpassword7" },
                    { 8, "hank@example.com", "Hank", "hashedpassword8" },
                    { 9, "ivy@example.com", "Ivy", "hashedpassword9" },
                    { 10, "jack@example.com", "Jack", "hashedpassword10" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
