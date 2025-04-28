using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Book.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class createProductModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListPrice = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Price50 = table.Column<double>(type: "float", nullable: false),
                    Price100 = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "Author", "ISBN", "ListPrice", "Price", "Price100", "Price50", "Title" },
                values: new object[,]
                {
                    { 1, "Hiroshi Sakurazaka", "978-0-123456-47-2", 29.989999999999998, 27.989999999999998, 23.989999999999998, 25.989999999999998, "Edge of Tomorrow" },
                    { 2, "Kip Thorne", "978-0-987654-32-1", 35.5, 32.5, 28.0, 30.0, "Interstellar: The Science" },
                    { 3, "James Clear", "978-0-123456-01-3", 25.0, 23.0, 19.0, 21.0, "Atomic Habits" },
                    { 4, "Robin Sharma", "978-0-123456-02-4", 22.989999999999998, 20.989999999999998, 16.989999999999998, 18.989999999999998, "The 5 AM Club" },
                    { 5, "Ernest Cline", "978-0-123456-03-5", 28.0, 26.0, 22.0, 24.0, "Ready Player One" },
                    { 6, "Andy Weir", "978-0-123456-04-6", 30.0, 28.0, 24.0, 26.0, "The Martian" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
