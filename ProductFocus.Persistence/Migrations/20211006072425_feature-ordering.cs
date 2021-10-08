using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductFocus.Persistence.Migrations
{
    public partial class featureordering : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeatureOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderingCategory = table.Column<int>(type: "int", nullable: false),
                    FeatureId = table.Column<long>(type: "bigint", nullable: false),
                    OrderNumber = table.Column<long>(type: "bigint", nullable: false),
                    SprintId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeatureOrders_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeatureOrders_FeatureId",
                table: "FeatureOrders",
                column: "FeatureId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeatureOrders");
        }
    }
}
