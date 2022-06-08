using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProductTests.Application.QueryHandler.GetTestRunQueries
{
    public class GetTestRunByIdQuery : IRequest<GetTestRunDto>
    {
        public long TestRunId { get; private set; }
        public GetTestRunByIdQuery(long testRunId)
        {
            TestRunId = testRunId;
        }
        internal sealed class GetTestRunByIdQueryHandler : IRequestHandler<GetTestRunByIdQuery, GetTestRunDto>
        {
            private readonly string _queriesConnectionString;
            public GetTestRunByIdQueryHandler(IConfiguration configuration)
            {
                _queriesConnectionString = configuration["ConnectionStrings:QueriesConnectionString"] ?? throw new NullReferenceException("Connection String is missing from config file");
            }

            public async Task<GetTestRunDto> Handle(GetTestRunByIdQuery request, CancellationToken cancellationToken)
            {
                GetTestRunDto testRun;

                string sql = @"SELECT trun.Id, tplan.Id AS PlanId, tplan.Name as Title, tplan.TestType FROM [producttest].[TestRuns] trun
                        INNER JOIN [producttest].[TestPlansVersion] tplan ON trun.TestPlanVersionId = tplan.Id WHERE trun.Id = @TestRunId;

                        SELECT tsuite.Id, tsuite.TestPlanVersionId AS TestPlanId, tsuite.Name as Title FROM [producttest].[TestSuitesVersion] tsuite
                        INNER JOIN [producttest].[TestRuns] trun ON trun.TestPlanVersionId = tsuite.TestPlanVersionId
                        WHERE trun.Id = @TestRunId;

                        SELECT tcase.Id, tsuite.Id AS TestSuiteId, tcase.Title, tcase.ResultStatus, tcase.IsIncluded FROM [producttest].[TestRuns] trun
                        INNER JOIN [producttest].[TestPlansVersion] tplan ON trun.TestPlanVersionId = tplan.Id
                        INNER JOIN [producttest].[TestSuitesVersion] tsuite ON tplan.Id = tsuite.TestPlanVersionId
                        INNER JOIN [producttest].[TestCasesVersion] tcase ON tcase.TestSuiteVersionId = tsuite.Id
                        WHERE trun.Id = @TestRunId;

                        SELECT tstep.Id, tcase.Id AS TestCaseId, tstep.StepNo, tstep.Action, tstep.ExpectedResult, tstep.ResultStatus FROM [producttest].[TestRuns] trun
                        INNER JOIN [ProductTest].[TestPlansVersion] tplan ON tplan.Id = trun.TestPlanVersionId
                        INNER JOIN [ProductTest].[TestSuitesVersion] tsuite ON tsuite.TestPlanVersionId = tplan.Id
                        INNER JOIN [ProductTest].[TestCasesVersion] tcase ON tcase.TestSuiteVersionId = tsuite.Id
                        INNER JOIN [ProductTest].[TestStepsVersion] tstep ON tcase.Id = tstep.TestCaseVersionId
                        WHERE trun.Id = @TestRunId;";

                using(IDbConnection con = new SqlConnection(_queriesConnectionString))
                {
                    var result = await con.QueryMultipleAsync(sql, new
                    {
                        request.TestRunId
                    });

                    testRun = (await result.ReadAsync<GetTestRunDto>()).SingleOrDefault();
                    var testSuites = await result.ReadAsync<GetTestRunSuiteDto>();
                    var testCases = await result.ReadAsync<GetTestRunCaseDto>();
                    var testSteps = await result.ReadAsync<GetTestRunStepDto>();

                    testRun.TestSuites = testSuites.Where(x => x.TestPlanId == testRun.PlanId).ToList();
                    foreach(GetTestRunSuiteDto suite in testSuites)
                    {
                        suite.TestCases = testCases.Where(x => x.TestSuiteId == suite.Id).ToList();
                        foreach(GetTestRunCaseDto testCase in suite.TestCases)
                        {
                            testCase.TestSteps = testSteps.Where(x => x.TestCaseId == testCase.Id).ToList();
                        }
                    }
                }
                return testRun;
            }
        }
    }
}
