using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductFocus.Persistence.Migrations
{
    public partial class featureentityupdatev1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkPgressIndicator",
                table: "Features",
                newName: "WorkCompletionPercentage");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Features",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UniqueWorkItemNumber",
                table: "Features",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniqueWorkItemNumber",
                table: "Features");

            migrationBuilder.RenameColumn(
                name: "WorkCompletionPercentage",
                table: "Features",
                newName: "WorkPgressIndicator");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Features",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
