using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductFocus.Persistence.Migrations
{
    public partial class release : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ReleaseId",
                table: "Features",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Releases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Releases", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Features_ReleaseId",
                table: "Features",
                column: "ReleaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Features_Releases_ReleaseId",
                table: "Features",
                column: "ReleaseId",
                principalTable: "Releases",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Features_Releases_ReleaseId",
                table: "Features");

            migrationBuilder.DropTable(
                name: "Releases");

            migrationBuilder.DropIndex(
                name: "IX_Features_ReleaseId",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "ReleaseId",
                table: "Features");
        }
    }
}
