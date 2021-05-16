using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductFocus.Persistence.Migrations
{
    public partial class sprintentitywithotherchangesv1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ObjectId",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SprintId",
                table: "Features",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Sprint",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sprint", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Features_SprintId",
                table: "Features",
                column: "SprintId");

            migrationBuilder.AddForeignKey(
                name: "FK_Features_Sprint_SprintId",
                table: "Features",
                column: "SprintId",
                principalTable: "Sprint",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Features_Sprint_SprintId",
                table: "Features");

            migrationBuilder.DropTable(
                name: "Sprint");

            migrationBuilder.DropIndex(
                name: "IX_Features_SprintId",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "ObjectId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SprintId",
                table: "Features");
        }
    }
}
