using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductFocus.Persistence.Migrations
{
    public partial class remarksaddedinfeature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FunctionalTestability",
                table: "Features",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "Features",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FunctionalTestability",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "Features");
        }
    }
}
