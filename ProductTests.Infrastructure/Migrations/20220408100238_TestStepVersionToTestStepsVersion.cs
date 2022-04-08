using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductTests.Infrastructure.Migrations
{
    public partial class TestStepVersionToTestStepsVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestStepVersion_TestCasesVersion_TestCaseVersionId",
                schema: "producttest",
                table: "TestStepVersion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestStepVersion",
                schema: "producttest",
                table: "TestStepVersion");

            migrationBuilder.RenameTable(
                name: "TestStepVersion",
                schema: "producttest",
                newName: "TestStepsVersion",
                newSchema: "producttest");

            migrationBuilder.RenameIndex(
                name: "IX_TestStepVersion_TestCaseVersionId",
                schema: "producttest",
                table: "TestStepsVersion",
                newName: "IX_TestStepsVersion_TestCaseVersionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestStepsVersion",
                schema: "producttest",
                table: "TestStepsVersion",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TestStepsVersion_TestCasesVersion_TestCaseVersionId",
                schema: "producttest",
                table: "TestStepsVersion",
                column: "TestCaseVersionId",
                principalSchema: "producttest",
                principalTable: "TestCasesVersion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestStepsVersion_TestCasesVersion_TestCaseVersionId",
                schema: "producttest",
                table: "TestStepsVersion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestStepsVersion",
                schema: "producttest",
                table: "TestStepsVersion");

            migrationBuilder.RenameTable(
                name: "TestStepsVersion",
                schema: "producttest",
                newName: "TestStepVersion",
                newSchema: "producttest");

            migrationBuilder.RenameIndex(
                name: "IX_TestStepsVersion_TestCaseVersionId",
                schema: "producttest",
                table: "TestStepVersion",
                newName: "IX_TestStepVersion_TestCaseVersionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestStepVersion",
                schema: "producttest",
                table: "TestStepVersion",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TestStepVersion_TestCasesVersion_TestCaseVersionId",
                schema: "producttest",
                table: "TestStepVersion",
                column: "TestCaseVersionId",
                principalSchema: "producttest",
                principalTable: "TestCasesVersion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
