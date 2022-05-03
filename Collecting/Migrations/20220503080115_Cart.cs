using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Collecting.Migrations
{
    public partial class Cart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StickersDb_CategoriesDb_categoryID",
                table: "StickersDb");

            migrationBuilder.RenameColumn(
                name: "categoryID",
                table: "StickersDb",
                newName: "CategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_StickersDb_categoryID",
                table: "StickersDb",
                newName: "IX_StickersDb_CategoryID");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "StickersDb",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "CartsDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartsDb", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Index = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    CartId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersDb", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CartItemsDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    StickerId = table.Column<int>(type: "int", nullable: false),
                    CartId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItemsDb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItemsDb_CartsDb_CartId",
                        column: x => x.CartId,
                        principalTable: "CartsDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItemsDb_StickersDb_StickerId",
                        column: x => x.StickerId,
                        principalTable: "StickersDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItemsDb_CartId",
                table: "CartItemsDb",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItemsDb_StickerId",
                table: "CartItemsDb",
                column: "StickerId");

            migrationBuilder.AddForeignKey(
                name: "FK_StickersDb_CategoriesDb_CategoryID",
                table: "StickersDb",
                column: "CategoryID",
                principalTable: "CategoriesDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StickersDb_CategoriesDb_CategoryID",
                table: "StickersDb");

            migrationBuilder.DropTable(
                name: "CartItemsDb");

            migrationBuilder.DropTable(
                name: "UsersDb");

            migrationBuilder.DropTable(
                name: "CartsDb");

            migrationBuilder.RenameColumn(
                name: "CategoryID",
                table: "StickersDb",
                newName: "categoryID");

            migrationBuilder.RenameIndex(
                name: "IX_StickersDb_CategoryID",
                table: "StickersDb",
                newName: "IX_StickersDb_categoryID");

            migrationBuilder.AlterColumn<int>(
                name: "Text",
                table: "StickersDb",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_StickersDb_CategoriesDb_categoryID",
                table: "StickersDb",
                column: "categoryID",
                principalTable: "CategoriesDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
