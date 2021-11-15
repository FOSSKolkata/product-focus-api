using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductFocus.Persistence.Migrations
{
    public partial class ProductIdInFeature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Features_Modules_ModuleId",
                table: "Features");

            migrationBuilder.AlterColumn<long>(
                name: "ModuleId",
                table: "WorkItemDomainEventLogs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "ModuleId",
                table: "Features",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "Features",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_Features_Modules_ModuleId",
                table: "Features",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Features_Modules_ModuleId",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Features");

            migrationBuilder.AlterColumn<long>(
                name: "ModuleId",
                table: "WorkItemDomainEventLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModuleId",
                table: "Features",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Features_Modules_ModuleId",
                table: "Features",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
