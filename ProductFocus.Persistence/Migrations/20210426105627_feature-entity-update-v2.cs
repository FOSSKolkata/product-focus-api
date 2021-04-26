using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductFocus.Persistence.Migrations
{
    public partial class featureentityupdatev2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Features_Modules_ModuleId1",
                table: "Features");

            migrationBuilder.DropIndex(
                name: "IX_Features_ModuleId1",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "ModuleId1",
                table: "Features");

            migrationBuilder.AlterColumn<long>(
                name: "ModuleId",
                table: "Features",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Features_ModuleId",
                table: "Features",
                column: "ModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Features_Modules_ModuleId",
                table: "Features",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Features_Modules_ModuleId",
                table: "Features");

            migrationBuilder.DropIndex(
                name: "IX_Features_ModuleId",
                table: "Features");

            migrationBuilder.AlterColumn<int>(
                name: "ModuleId",
                table: "Features",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "ModuleId1",
                table: "Features",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Features_ModuleId1",
                table: "Features",
                column: "ModuleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Features_Modules_ModuleId1",
                table: "Features",
                column: "ModuleId1",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
