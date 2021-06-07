using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductFocus.Persistence.Migrations
{
    public partial class invitationentityupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OrganizationId",
                table: "Invitations",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_OrganizationId",
                table: "Invitations",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Organizations_OrganizationId",
                table: "Invitations",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Organizations_OrganizationId",
                table: "Invitations");

            migrationBuilder.DropIndex(
                name: "IX_Invitations_OrganizationId",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Invitations");
        }
    }
}
