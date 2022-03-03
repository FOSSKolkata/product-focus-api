using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductDocumentations.Infrastructure.Migrations
{
    public partial class documentsoftdelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                schema: "productdocumentation",
                table: "ProductDocumentations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "productdocumentation",
                table: "ProductDocumentations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "productdocumentation",
                table: "ProductDocumentations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "productdocumentation",
                table: "ProductDocumentations");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "productdocumentation",
                table: "ProductDocumentations");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "productdocumentation",
                table: "ProductDocumentations");
        }
    }
}
