using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductFocus.Persistence.Migrations
{
    public partial class tablerename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeatureComment_Features_FeatureId",
                table: "FeatureComment");

            migrationBuilder.DropForeignKey(
                name: "FK_Features_Module_ModuleId1",
                table: "Features");

            migrationBuilder.DropForeignKey(
                name: "FK_Member_Organizations_OrganizationId",
                table: "Member");

            migrationBuilder.DropForeignKey(
                name: "FK_Member_Users_UserId",
                table: "Member");

            migrationBuilder.DropForeignKey(
                name: "FK_Module_Products_ProductId",
                table: "Module");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermission_Permissions_PermissionId",
                table: "RolePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermission_Roles_RoleId",
                table: "RolePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Member_MemberId",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_Features_FeatureId",
                table: "Task");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Task",
                table: "Task");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RolePermission",
                table: "RolePermission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Module",
                table: "Module");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Member",
                table: "Member");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeatureComment",
                table: "FeatureComment");

            migrationBuilder.RenameTable(
                name: "Task",
                newName: "Tasks");

            migrationBuilder.RenameTable(
                name: "RolePermission",
                newName: "RolePermissions");

            migrationBuilder.RenameTable(
                name: "Module",
                newName: "Modules");

            migrationBuilder.RenameTable(
                name: "Member",
                newName: "Members");

            migrationBuilder.RenameTable(
                name: "FeatureComment",
                newName: "FeatureComments");

            migrationBuilder.RenameIndex(
                name: "IX_Task_FeatureId",
                table: "Tasks",
                newName: "IX_Tasks_FeatureId");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermission_RoleId",
                table: "RolePermissions",
                newName: "IX_RolePermissions_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermission_PermissionId",
                table: "RolePermissions",
                newName: "IX_RolePermissions_PermissionId");

            migrationBuilder.RenameIndex(
                name: "IX_Module_ProductId",
                table: "Modules",
                newName: "IX_Modules_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Member_UserId",
                table: "Members",
                newName: "IX_Members_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Member_OrganizationId",
                table: "Members",
                newName: "IX_Members_OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_FeatureComment_FeatureId",
                table: "FeatureComments",
                newName: "IX_FeatureComments_FeatureId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Modules",
                table: "Modules",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Members",
                table: "Members",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeatureComments",
                table: "FeatureComments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureComments_Features_FeatureId",
                table: "FeatureComments",
                column: "FeatureId",
                principalTable: "Features",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Features_Modules_ModuleId1",
                table: "Features",
                column: "ModuleId1",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Organizations_OrganizationId",
                table: "Members",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Users_UserId",
                table: "Members",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Products_ProductId",
                table: "Modules",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Permissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Roles_RoleId",
                table: "RolePermissions",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Members_MemberId",
                table: "Roles",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Features_FeatureId",
                table: "Tasks",
                column: "FeatureId",
                principalTable: "Features",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeatureComments_Features_FeatureId",
                table: "FeatureComments");

            migrationBuilder.DropForeignKey(
                name: "FK_Features_Modules_ModuleId1",
                table: "Features");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_Organizations_OrganizationId",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_Users_UserId",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Products_ProductId",
                table: "Modules");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Permissions_PermissionId",
                table: "RolePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Roles_RoleId",
                table: "RolePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Members_MemberId",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Features_FeatureId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Modules",
                table: "Modules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Members",
                table: "Members");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeatureComments",
                table: "FeatureComments");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "Task");

            migrationBuilder.RenameTable(
                name: "RolePermissions",
                newName: "RolePermission");

            migrationBuilder.RenameTable(
                name: "Modules",
                newName: "Module");

            migrationBuilder.RenameTable(
                name: "Members",
                newName: "Member");

            migrationBuilder.RenameTable(
                name: "FeatureComments",
                newName: "FeatureComment");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_FeatureId",
                table: "Task",
                newName: "IX_Task_FeatureId");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermissions_RoleId",
                table: "RolePermission",
                newName: "IX_RolePermission_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermission",
                newName: "IX_RolePermission_PermissionId");

            migrationBuilder.RenameIndex(
                name: "IX_Modules_ProductId",
                table: "Module",
                newName: "IX_Module_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Members_UserId",
                table: "Member",
                newName: "IX_Member_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Members_OrganizationId",
                table: "Member",
                newName: "IX_Member_OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_FeatureComments_FeatureId",
                table: "FeatureComment",
                newName: "IX_FeatureComment_FeatureId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Task",
                table: "Task",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolePermission",
                table: "RolePermission",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Module",
                table: "Module",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Member",
                table: "Member",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeatureComment",
                table: "FeatureComment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureComment_Features_FeatureId",
                table: "FeatureComment",
                column: "FeatureId",
                principalTable: "Features",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Features_Module_ModuleId1",
                table: "Features",
                column: "ModuleId1",
                principalTable: "Module",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Member_Organizations_OrganizationId",
                table: "Member",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Member_Users_UserId",
                table: "Member",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Module_Products_ProductId",
                table: "Module",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermission_Permissions_PermissionId",
                table: "RolePermission",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermission_Roles_RoleId",
                table: "RolePermission",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Member_MemberId",
                table: "Roles",
                column: "MemberId",
                principalTable: "Member",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Features_FeatureId",
                table: "Task",
                column: "FeatureId",
                principalTable: "Features",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
