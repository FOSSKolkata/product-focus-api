using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessRequirements.Infrastructure.Migrations
{
    public partial class refactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "businessrequirement");

            migrationBuilder.CreateSequence(
                name: "EntityFrameworkHiLoSequence",
                schema: "businessrequirement",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "Organizations",
                schema: "businessrequirement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                schema: "businessrequirement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "businessrequirement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalSchema: "businessrequirement",
                        principalTable: "Organizations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BusinessRequirementTags",
                schema: "businessrequirement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    BusinessRequirementId = table.Column<long>(type: "bigint", nullable: false),
                    TagId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessRequirementTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessRequirementTags_Tags_TagId",
                        column: x => x.TagId,
                        principalSchema: "businessrequirement",
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessRequirements",
                schema: "businessrequirement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceivedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SourceEnum = table.Column<int>(type: "int", nullable: false),
                    SourceInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessRequirements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessRequirements_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "businessrequirement",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessRequirementAttachments",
                schema: "businessrequirement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    BusinessRequirementId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_BusinessRequirementAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessRequirementAttachments_BusinessRequirements_BusinessRequirementId",
                        column: x => x.BusinessRequirementId,
                        principalSchema: "businessrequirement",
                        principalTable: "BusinessRequirements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRequirementAttachments_BusinessRequirementId",
                schema: "businessrequirement",
                table: "BusinessRequirementAttachments",
                column: "BusinessRequirementId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRequirements_ProductId",
                schema: "businessrequirement",
                table: "BusinessRequirements",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRequirementTags_TagId",
                schema: "businessrequirement",
                table: "BusinessRequirementTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_OrganizationId",
                schema: "businessrequirement",
                table: "Products",
                column: "OrganizationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessRequirementAttachments",
                schema: "businessrequirement");

            migrationBuilder.DropTable(
                name: "BusinessRequirementTags",
                schema: "businessrequirement");

            migrationBuilder.DropTable(
                name: "BusinessRequirements",
                schema: "businessrequirement");

            migrationBuilder.DropTable(
                name: "Tags",
                schema: "businessrequirement");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "businessrequirement");

            migrationBuilder.DropTable(
                name: "Organizations",
                schema: "businessrequirement");

            migrationBuilder.DropSequence(
                name: "EntityFrameworkHiLoSequence",
                schema: "businessrequirement");
        }
    }
}
