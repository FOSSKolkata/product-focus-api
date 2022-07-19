using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductFocus.Persistence.Migrations
{
    public partial class uniqueuserobjectid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessRequirementAttachments");

            migrationBuilder.DropTable(
                name: "BusinessRequirementTags");

            migrationBuilder.DropTable(
                name: "BusinessRequirements");

            migrationBuilder.AlterColumn<string>(
                name: "ObjectId",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ObjectId",
                table: "Users",
                column: "ObjectId",
                unique: true,
                filter: "[ObjectId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_ObjectId",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "ObjectId",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "BusinessRequirements",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReceivedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SourceEnum = table.Column<int>(type: "int", nullable: false),
                    SourceInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessRequirements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessRequirements_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessRequirementTags",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    TagId = table.Column<long>(type: "bigint", nullable: false),
                    BusinessRequirementId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessRequirementTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessRequirementTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessRequirementAttachments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    BusinessRequirementId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Uri = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessRequirementAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessRequirementAttachments_BusinessRequirements_BusinessRequirementId",
                        column: x => x.BusinessRequirementId,
                        principalTable: "BusinessRequirements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRequirementAttachments_BusinessRequirementId",
                table: "BusinessRequirementAttachments",
                column: "BusinessRequirementId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRequirements_ProductId",
                table: "BusinessRequirements",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRequirementTags_TagId",
                table: "BusinessRequirementTags",
                column: "TagId");
        }
    }
}
