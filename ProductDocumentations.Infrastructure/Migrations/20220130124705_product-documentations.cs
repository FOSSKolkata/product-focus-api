using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductDocumentations.Infrastructure.Migrations
{
    public partial class productdocumentations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "productdoc");

            migrationBuilder.CreateSequence(
                name: "EntityFrameworkHiLoSequence",
                schema: "productdoc",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "ProductDocumentations",
                schema: "productdoc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "ProductDocumentationAttachment",
                schema: "productdoc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    table.PrimaryKey("PK_ProductDocumentationAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDocumentationAttachment_ProductDocumentations_ProductDocumentationId",
                        column: x => x.ProductDocumentationId,
                        principalSchema: "productdoc",
                        principalTable: "ProductDocumentations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductDocumentationAttachment_ProductDocumentationId",
                schema: "productdoc",
                table: "ProductDocumentationAttachment",
                column: "ProductDocumentationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductDocumentationAttachment",
                schema: "productdoc");

            migrationBuilder.DropTable(
                name: "ProductDocumentations",
                schema: "productdoc");

            migrationBuilder.DropSequence(
                name: "EntityFrameworkHiLoSequence",
                schema: "productdoc");
        }
    }
}
