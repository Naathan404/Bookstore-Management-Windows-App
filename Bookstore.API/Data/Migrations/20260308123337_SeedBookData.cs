using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedBookData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookId", "Author", "Category", "Description", "Price", "StockQuantity", "Title" },
                values: new object[,]
                {
                    { 1, "Aoyama Gosho", "Truyện tranh", "", 25000m, 50, "Conan Tập 100" },
                    { 2, "NXB Trẻ", "Giáo trình", "", 120000m, 20, "Lập trình C#" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 2);
        }
    }
}
