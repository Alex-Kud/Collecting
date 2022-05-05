using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Collecting.Migrations
{
    public partial class OrderChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "OrdersDb",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrdersDb_CartId",
                table: "OrdersDb",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersDb_UserId",
                table: "OrdersDb",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersDb_CartsDb_CartId",
                table: "OrdersDb",
                column: "CartId",
                principalTable: "CartsDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersDb_UsersDb_UserId",
                table: "OrdersDb",
                column: "UserId",
                principalTable: "UsersDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdersDb_CartsDb_CartId",
                table: "OrdersDb");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdersDb_UsersDb_UserId",
                table: "OrdersDb");

            migrationBuilder.DropIndex(
                name: "IX_OrdersDb_CartId",
                table: "OrdersDb");

            migrationBuilder.DropIndex(
                name: "IX_OrdersDb_UserId",
                table: "OrdersDb");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "OrdersDb");
        }
    }
}
