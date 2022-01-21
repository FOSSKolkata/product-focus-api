using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductFocus.Persistence.Migrations
{
    public partial class attachmentname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "BusinessRequirementAttachments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRequirementAttachments_BusinessRequirementId",
                table: "BusinessRequirementAttachments",
                column: "BusinessRequirementId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessRequirementAttachments_BusinessRequirements_BusinessRequirementId",
                table: "BusinessRequirementAttachments",
                column: "BusinessRequirementId",
                principalTable: "BusinessRequirements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessRequirementAttachments_BusinessRequirements_BusinessRequirementId",
                table: "BusinessRequirementAttachments");

            migrationBuilder.DropIndex(
                name: "IX_BusinessRequirementAttachments_BusinessRequirementId",
                table: "BusinessRequirementAttachments");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "BusinessRequirementAttachments");
        }
    }
}
