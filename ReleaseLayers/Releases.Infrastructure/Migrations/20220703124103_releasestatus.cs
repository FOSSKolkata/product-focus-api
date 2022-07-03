using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Releases.Infrastructure.Migrations
{
    public partial class releasestatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "release",
                table: "Releases",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                schema: "release",
                table: "Releases");
        }
    }
}
