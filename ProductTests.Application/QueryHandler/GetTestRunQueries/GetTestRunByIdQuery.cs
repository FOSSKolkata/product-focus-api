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
        public long TestPlanId { get; private set; }
        public GetTestRunByIdQuery(long testPlanId)
        {
            TestPlanId = testPlanId;
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

                string sql = @"SELECT tplan.Id, tplan.Name as Title, tplan.TestType FROM [producttest].[TestPlansVersion] tplan WHERE tplan.Id =  @TestPlanId;

                    SELECT tsuite.Id,  tsuite.TestPlanVersionId AS TestPlanId, tsuite.Name as Title FROM ProductTest.TestSuitesVersion tsuite
                    WHERE tsuite.TestPlanVersionId = @TestPlanId;

                    SELECT tcase.Id, tsuite.Id AS TestSuiteId, tcase.Title, tcase.ResultStatus FROM ProductTest.TestSuiteTestCaseMappingsVersion tstcm
                    INNER JOIN ProductTest.TestSuitesVersion tsuite ON tstcm.TestSuiteVersionId = tsuite.Id
                    INNER JOIN ProductTest.TestCasesVersion tcase ON tstcm.TestCaseVersionId = tcase.Id
                    WHERE tsuite.TestPlanVersionId =  @TestPlanId;

                    SELECT tstep.Id, tcase.Id AS TestCaseId, tstep.StepNo, tstep.Action, tstep.ExpectedResult, tstep.ResultStatus FROM [ProductTest].[TestSuiteTestCaseMappingsVersion] tstcm
                    INNER JOIN [ProductTest].[TestSuitesVersion] tsuite ON tstcm.TestSuiteVersionId = tsuite.Id
                    INNER JOIN [ProductTest].[TestCasesVersion] tcase ON tstcm.TestCaseVersionId = tcase.Id
                    INNER JOIN [ProductTest].[TestStepsVersion] tstep ON tcase.Id = tstep.TestCaseVersionId
                    WHERE tsuite.TestPlanVersionId =  @TestPlanId;";

                using(IDbConnection con = new SqlConnection(_queriesConnectionString))
                {
                    var result = await con.QueryMultipleAsync(sql, new
                    {
                        request.TestPlanId
                    });

                    testRun = (await result.ReadAsync<GetTestRunDto>()).SingleOrDefault();
                    var testSuites = await result.ReadAsync<GetTestRunSuiteDto>();
                    var testCases = await result.ReadAsync<GetTestRunCaseDto>();
                    var testSteps = await result.ReadAsync<GetTestRunStepDto>();

                    testRun.TestSuites = testSuites.Where(x => x.TestPlanId == request.TestPlanId).ToList();
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
