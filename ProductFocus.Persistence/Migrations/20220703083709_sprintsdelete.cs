using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductFocus.Persistence.Migrations
{
    public partial class sprintsdelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Sprint",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Sprint",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Sprint",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Sprint");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Sprint");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Sprint");
        }
    }
}
