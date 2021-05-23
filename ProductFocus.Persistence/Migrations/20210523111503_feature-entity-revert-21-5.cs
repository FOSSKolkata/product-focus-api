using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductFocus.Persistence.Migrations
{
    public partial class featureentityrevert215 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Features_FeatureId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_FeatureId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FeatureId",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FeatureId",
                table: "Users",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_FeatureId",
                table: "Users",
                column: "FeatureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Features_FeatureId",
                table: "Users",
                column: "FeatureId",
                principalTable: "Features",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
