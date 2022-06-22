using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Releases.Infrastructure.Migrations
{
    public partial class release : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "release");

            migrationBuilder.CreateSequence(
                name: "EntityFrameworkHiLoSequence",
                schema: "release",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "Releases",
                schema: "release",
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

            migrationBuilder.CreateTable(
                name: "ReleaseWorkItemCounts",
                schema: "release",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    WorkItemType = table.Column<int>(type: "int", nullable: false),
                    WorkItemCount = table.Column<long>(type: "bigint", nullable: false),
                    ReleaseId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseWorkItemCounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReleaseWorkItemCounts_Releases_ReleaseId",
                        column: x => x.ReleaseId,
                        principalSchema: "release",
                        principalTable: "Releases",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseWorkItemCounts_ReleaseId",
                schema: "release",
                table: "ReleaseWorkItemCounts",
                column: "ReleaseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReleaseWorkItemCounts",
                schema: "release");

            migrationBuilder.DropTable(
                name: "Releases",
                schema: "release");

            migrationBuilder.DropSequence(
                name: "EntityFrameworkHiLoSequence",
                schema: "release");
        }
    }
}
