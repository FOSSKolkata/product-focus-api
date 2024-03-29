﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductTests.Infrastructure.Migrations
{
    public partial class testrun : Migration
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
                name: "TestPlansVersion",
                schema: "producttest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestType = table.Column<int>(type: "int", nullable: false),
                    ProductDocumentationId = table.Column<long>(type: "bigint", nullable: false),
                    WorkItemId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestPlansVersion", x => x.Id);
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
                name: "TestSuites",
                schema: "producttest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestPlanId = table.Column<long>(type: "bigint", nullable: false),
                    OrderNo = table.Column<long>(type: "bigint", nullable: false),
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
                name: "TestRuns",
                schema: "producttest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    SprintId = table.Column<long>(type: "bigint", nullable: true),
                    TestPlanId = table.Column<long>(type: "bigint", nullable: false),
                    RunningStatus = table.Column<int>(type: "int", nullable: false),
                    TestPlanVersionId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestRuns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestRuns_TestPlansVersion_TestPlanVersionId",
                        column: x => x.TestPlanVersionId,
                        principalSchema: "producttest",
                        principalTable: "TestPlansVersion",
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
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TestSuiteTestCaseMappings_TestSuites_TestSuiteId",
                        column: x => x.TestSuiteId,
                        principalSchema: "producttest",
                        principalTable: "TestSuites",
                        principalColumn: "Id");
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
                    TestSuiteVersionId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestCasesVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestCasesVersion_TestSuitesVersion_TestSuiteVersionId",
                        column: x => x.TestSuiteVersionId,
                        principalSchema: "producttest",
                        principalTable: "TestSuitesVersion",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TestStepsVersion",
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
                    table.PrimaryKey("PK_TestStepsVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestStepsVersion_TestCasesVersion_TestCaseVersionId",
                        column: x => x.TestCaseVersionId,
                        principalSchema: "producttest",
                        principalTable: "TestCasesVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestCasesVersion_TestSuiteVersionId",
                schema: "producttest",
                table: "TestCasesVersion",
                column: "TestSuiteVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestRuns_TestPlanVersionId",
                schema: "producttest",
                table: "TestRuns",
                column: "TestPlanVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestSteps_TestCaseId",
                schema: "producttest",
                table: "TestSteps",
                column: "TestCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_TestStepsVersion_TestCaseVersionId",
                schema: "producttest",
                table: "TestStepsVersion",
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestRuns",
                schema: "producttest");

            migrationBuilder.DropTable(
                name: "TestSteps",
                schema: "producttest");

            migrationBuilder.DropTable(
                name: "TestStepsVersion",
                schema: "producttest");

            migrationBuilder.DropTable(
                name: "TestSuiteTestCaseMappings",
                schema: "producttest");

            migrationBuilder.DropTable(
                name: "TestCasesVersion",
                schema: "producttest");

            migrationBuilder.DropTable(
                name: "TestCases",
                schema: "producttest");

            migrationBuilder.DropTable(
                name: "TestSuites",
                schema: "producttest");

            migrationBuilder.DropTable(
                name: "TestSuitesVersion",
                schema: "producttest");

            migrationBuilder.DropTable(
                name: "TestPlans",
                schema: "producttest");

            migrationBuilder.DropTable(
                name: "TestPlansVersion",
                schema: "producttest");

            migrationBuilder.DropSequence(
                name: "EntityFrameworkHiLoSequence",
                schema: "producttest");
        }
    }
}
