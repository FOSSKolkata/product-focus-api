using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductFocus.Persistence.Migrations
{
    public partial class featureentityupdate215 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FeatureId",
                table: "Users",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "Features",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "StoryPoint",
                table: "Features",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "StoryPoint",
                table: "Features");
        }
    }
}
