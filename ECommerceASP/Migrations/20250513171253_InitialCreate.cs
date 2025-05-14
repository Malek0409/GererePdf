using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerceASP.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "name" },
                values: new object[,]
                {
                    { 1, "Chaussure" },
                    { 2, "Pantalon" },
                    { 3, "Chemise" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, "Nike Air", 120.00m },
                    { 2, 1, "Adidas Run", 100.00m },
                    { 3, 1, "Puma Sport", 90.00m },
                    { 4, 1, "Reebok Classic", 110.00m },
                    { 5, 1, "Converse All Star", 80.00m },
                    { 6, 2, "Jean Slim", 60.00m },
                    { 7, 2, "Chino Beige", 55.00m },
                    { 8, 2, "Jogging Noir", 40.00m },
                    { 9, 2, "Short Sport", 35.00m },
                    { 10, 2, "Pantalon Cargo", 70.00m },
                    { 11, 3, "Chemise Blanche", 45.00m },
                    { 12, 3, "Chemise à Carreaux", 50.00m },
                    { 13, 3, "Chemise Jean", 55.00m },
                    { 14, 3, "Chemise Lin", 60.00m },
                    { 15, 3, "Chemise Noire", 48.00m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
