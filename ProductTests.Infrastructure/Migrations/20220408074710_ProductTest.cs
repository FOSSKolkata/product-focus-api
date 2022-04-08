using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductTests.Infrastructure.Migrations
{
    public partial class ProductTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "producttest");

            migrationBuilder.CreateSequence(
                name: "EntityFrameworkHiLoSequence",
                schema: "producttest",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "TestCases",
                schema: "producttest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Preconditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_TestCases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestCasesVersion",
                schema: "producttest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Preconditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsIncluded = table.Column<bool>(type: "bit", nullable: false),
                    ResultStatus = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_TestCasesVersion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestPlans",
                schema: "producttest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    SprintId = table.Column<long>(type: "bigint", nullable: true),
                    TestType = table.Column<int>(type: "int", nullable: false),
                    ProductDocumentationId = table.Column<long>(type: "bigint", nullable: false),
                    WorkItemId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_TestPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestSteps",
                schema: "producttest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    TestCaseId = table.Column<long>(type: "bigint", nullable: false),
                    StepNo = table.Column<long>(type: "bigint", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpectedResult = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_TestSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestSteps_TestCases_TestCaseId",
                        column: x => x.TestCaseId,
                        principalSchema: "producttest",
                        principalTable: "TestCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestStepVersion",
                schema: "producttest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    TestCaseVersionId = table.Column<long>(type: "bigint", nullable: false),
                    StepNo = table.Column<long>(type: "bigint", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpectedResult = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestStepVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestStepVersion_TestCasesVersion_TestCaseVersionId",
                        column: x => x.TestCaseVersionId,
                        principalSchema: "producttest",
                        principalTable: "TestCasesVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestPlansVersion",
                schema: "producttest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestPlanId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    SprintId = table.Column<long>(type: "bigint", nullable: true),
                    TestType = table.Column<int>(type: "int", nullable: false),
                    ProductDocumentationId = table.Column<long>(type: "bigint", nullable: false),
                    WorkItemId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RunningStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestPlansVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestPlansVersion_TestPlans_TestPlanId",
                        column: x => x.TestPlanId,
                        principalSchema: "producttest",
                        principalTable: "TestPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestSuites",
                schema: "producttest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestPlanId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_TestSuites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestSuites_TestPlans_TestPlanId",
                        column: x => x.TestPlanId,
                        principalSchema: "producttest",
                        principalTable: "TestPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestSuitesVersion",
                schema: "producttest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestPlanVersionId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestSuitesVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestSuitesVersion_TestPlansVersion_TestPlanVersionId",
                        column: x => x.TestPlanVersionId,
                        principalSchema: "producttest",
                        principalTable: "TestPlansVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestSuiteTestCaseMappings",
                schema: "producttest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    TestSuiteId = table.Column<long>(type: "bigint", nullable: true),
                    TestCaseId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_TestSuiteTestCaseMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestSuiteTestCaseMappings_TestCases_TestCaseId",
                        column: x => x.TestCaseId,
                        principalSchema: "producttest",
                        principalTable: "TestCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestSuiteTestCaseMappings_TestSuites_TestSuiteId",
                        column: x => x.TestSuiteId,
                        principalSchema: "producttest",
                        principalTable: "TestSuites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestSuiteTestCaseMappingsVersion",
                schema: "producttest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    TestSuiteVersionId = table.Column<long>(type: "bigint", nullable: true),
                    TestCaseVersionId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestSuiteTestCaseMappingsVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestSuiteTestCaseMappingsVersion_TestCasesVersion_TestCaseVersionId",
                        column: x => x.TestCaseVersionId,
                        principalSchema: "producttest",
                        principalTable: "TestCasesVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestSuiteTestCaseMappingsVersion_TestSuitesVersion_TestSuiteVersionId",
                        column: x => x.TestSuiteVersionId,
                        principalSchema: "producttest",
                        principalTable: "TestSuitesVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestPlansVersion_TestPlanId",
                schema: "producttest",
                table: "TestPlansVersion",
                column: "TestPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TestSteps_TestCaseId",
                schema: "producttest",
                table: "TestSteps",
                column: "TestCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_TestStepVersion_TestCaseVersionId",
                schema: "producttest",
                table: "TestStepVersion",
                column: "TestCaseVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestSuites_TestPlanId",
                schema: "producttest",
                table: "TestSuites",
                column: "TestPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TestSuitesVersion_TestPlanVersionId",
                schema: "producttest",
                table: "TestSuitesVersion",
                column: "TestPlanVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestSuiteTestCaseMappings_TestCaseId",
                schema: "producttest",
                table: "TestSuiteTestCaseMappings",
                column: "TestCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_TestSuiteTestCaseMappings_TestSuiteId",
                schema: "producttest",
                table: "TestSuiteTestCaseMappings",
                column: "TestSuiteId");

            migrationBuilder.CreateIndex(
                name: "IX_TestSuiteTestCaseMappingsVersion_TestCaseVersionId",
                schema: "producttest",
                table: "TestSuiteTestCaseMappingsVersion",
                column: "TestCaseVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestSuiteTestCaseMappingsVersion_TestSuiteVersionId",
                schema: "producttest",
                table: "TestSuiteTestCaseMappingsVersion",
                column: "TestSuiteVersionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestSteps",
                schema: "producttest");

            migrationBuilder.DropTable(
                name: "TestStepVersion",
                schema: "producttest");

            migrationBuilder.DropTable(
                name: "TestSuiteTestCaseMappings",
                schema: "producttest");

            migrationBuilder.DropTable(
                name: "TestSuiteTestCaseMappingsVersion",
                schema: "producttest");

            migrationBuilder.DropTable(
                name: "TestCases",
                schema: "producttest");

            migrationBuilder.DropTable(
                name: "TestSuites",
                schema: "producttest");

            migrationBuilder.DropTable(
                name: "TestCasesVersion",
                schema: "producttest");

            migrationBuilder.DropTable(
                name: "TestSuitesVersion",
                schema: "producttest");

            migrationBuilder.DropTable(
                name: "TestPlansVersion",
                schema: "producttest");

            migrationBuilder.DropTable(
                name: "TestPlans",
                schema: "producttest");

            migrationBuilder.DropSequence(
                name: "EntityFrameworkHiLoSequence",
                schema: "producttest");
        }
    }
}
