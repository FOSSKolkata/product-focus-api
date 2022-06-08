using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using ProductTests.Domain.Model.TestRunAggregate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProductTests.Application.QueryHandler.GetTestRunQueries
{
    public class GetTestRunsQuery : IRequest<List<GetTestRunsDto>>
    {
        public long ProductId { get; private set; }
        public GetTestRunsQuery(long productId)
        {
            ProductId = productId;
        }
        internal sealed class GetTestRunsQueryHandler : IRequestHandler<GetTestRunsQuery, List<GetTestRunsDto>>
        {
            private readonly string _queriesConnectionString;
            public GetTestRunsQueryHandler(IConfiguration configuration)
            {
                _queriesConnectionString = configuration["ConnectionStrings:QueriesConnectionString"] ?? throw new NullReferenceException("Connection String is missing from config file");
            }

            public async Task<List<GetTestRunsDto>> Handle(GetTestRunsQuery request, CancellationToken cancellationToken)
            {
                List<GetTestRunsDto> testRuns;
                /*string sql = @"SELECT tplan.Id, tplan.Name as Title, tplan.TestType, COUNT(tcase.Id) AS Total FROM ProductTest.TestPlansVersion tplan
                    INNER JOIN ProductTest.TestSuitesVersion tsuite ON tplan.Id = tsuite.TestPlanVersionId
                    INNER JOIN ProductTest.TestSuiteTestCaseMappingsVersion tstcm ON tsuite.Id = tstcm.TestSuiteVersionId
                    INNER JOIN ProductTest.TestCasesVersion tcase ON tstcm.TestCaseVersionId = tcase.Id WHERE tplan.ProductId = @ProductId
                    GROUP BY tplan.Id, tplan.Name, tplan.TestType;";*/
                string sql = @"SELECT tplan.Id, tplan.Name as Title, tplan.TestType FROM ProductTest.TestPlansVersion tplan WHERE tplan.ProductId = @ProductId;

                    SELECT tsuite.Id, tsuite.Name, tsuite.TestPlanVersionId AS PlanId FROM ProductTest.TestSuitesVersion tsuite
                    WHERE tsuite.TestPlanVersionId in (SELECT tplan.Id FROM ProductTest.TestPlansVersion tplan WHERE tplan.ProductId = @ProductId);

                    SELECT tcase.Id, tsuite.Id AS SuiteId, tcase.Title, tcase.ResultStatus FROM ProductTest.TestSuiteTestCaseMappingsVersion tstcm
                    INNER JOIN ProductTest.TestSuitesVersion tsuite ON tstcm.TestSuiteVersionId = tsuite.Id
                    INNER JOIN ProductTest.TestCasesVersion tcase ON tstcm.TestCaseVersionId = tcase.Id
                    WHERE tsuite.TestPlanVersionId in (SELECT tplan.Id FROM ProductTest.TestPlansVersion tplan WHERE tplan.ProductId = @ProductId);";

                using(IDbConnection con = new SqlConnection(_queriesConnectionString))
                {
                    /*testRuns = (await con.QueryAsync<GetTestRunsDto>(sql, new {
                        request.ProductId
                    })).ToList();*/
                    var result = await con.QueryMultipleAsync(sql, new
                    {
                        request.ProductId
                    });
                    var testPlans = await result.ReadAsync<GetTestRunsDto>();
                    var testSuites = await result.ReadAsync<GetTestSuiteDto>();
                    var testCases = await result.ReadAsync<GetTestCaseDto>();

                    testRuns = testPlans.ToList();

                    foreach(GetTestRunsDto plan in testPlans)
                    {
                        var totalSuite = testSuites.Where(x => x.PlanId == plan.Id);
                        plan.TotalTestSuites = totalSuite.Count();
                        plan.TotalTestCases = 0;
                        plan.TotalPassedCases = 0;
                        plan.TotalFailedCases = 0;
                        foreach(GetTestSuiteDto suite in totalSuite)
                        {
                            plan.TotalTestCases += testCases.Where(x => x.SuiteId == suite.Id).Count();
                            plan.TotalPassedCases += testCases.Where(x => x.SuiteId == suite.Id && x.ResultStatus == TestCaseResult.Success).Count();
                            plan.TotalFailedCases += testCases.Where(x => x.SuiteId == suite.Id && x.ResultStatus == TestCaseResult.Failed).Count();
                        }
                    }
                }
                return testRuns;
            }
        }
    }
}
