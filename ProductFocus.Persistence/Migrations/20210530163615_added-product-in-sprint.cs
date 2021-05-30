using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductFocus.Persistence.Migrations
{
    public partial class addedproductinsprint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "Sprint",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Sprint_ProductId",
                table: "Sprint",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sprint_Products_ProductId",
                table: "Sprint",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sprint_Products_ProductId",
                table: "Sprint");

            migrationBuilder.DropIndex(
                name: "IX_Sprint_ProductId",
                table: "Sprint");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Sprint");
        }
    }
}
