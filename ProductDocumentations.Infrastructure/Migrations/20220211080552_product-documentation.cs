using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductDocumentations.Infrastructure.Migrations
{
    public partial class productdocumentation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "productdocumentation");

            migrationBuilder.CreateSequence(
                name: "EntityFrameworkHiLoSequence",
                schema: "productdocumentation",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "ProductDocumentations",
                schema: "productdocumentation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderNumber = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDocumentations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductDocumentationAttachments",
                schema: "productdocumentation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ProductDocumentationId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Uri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDocumentationAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDocumentationAttachments_ProductDocumentations_ProductDocumentationId",
                        column: x => x.ProductDocumentationId,
                        principalSchema: "productdocumentation",
                        principalTable: "ProductDocumentations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductDocumentationAttachments_ProductDocumentationId",
                schema: "productdocumentation",
                table: "ProductDocumentationAttachments",
                column: "ProductDocumentationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductDocumentationAttachments",
                schema: "productdocumentation");

            migrationBuilder.DropTable(
                name: "ProductDocumentations",
                schema: "productdocumentation");

            migrationBuilder.DropSequence(
                name: "EntityFrameworkHiLoSequence",
                schema: "productdocumentation");
        }
    }
}
