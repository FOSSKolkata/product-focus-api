using CSharpFunctionalExtensions;
using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProductTests.Application.QueryHandler.GetTestPlanQueries
{
    public sealed class GetTestPlanDetailsQuery : IRequest<Result<GetTestPlanDetailsDto>>
    {
        public long Id { get; private set; }
        public long ProductId { get; private set; }
        public GetTestPlanDetailsQuery(long productId, long id)
        {
            ProductId = productId;
            Id = id;
        }
        internal sealed class GetTestPlanDetailsQueryHandler : IRequestHandler<GetTestPlanDetailsQuery, Result<GetTestPlanDetailsDto>>
        {
            private readonly string _queriesConnectionString;
            public GetTestPlanDetailsQueryHandler(IConfiguration configuration)
            {
                _queriesConnectionString = configuration["ConnectionStrings:QueriesConnectionString"] ?? throw new NullReferenceException("Connection String is missing from config file");
            }
            public async Task<Result<GetTestPlanDetailsDto>> Handle(GetTestPlanDetailsQuery request, CancellationToken cancellationToken)
            {
                GetTestPlanDetailsDto testPlanDetails;
                string sql = @"SELECT tplan.Id as TestPlanId,
                    tplan.TestType, tplan.Name as TestPlanTitle
                    FROM producttest.TestPlans tplan
                    WHERE tplan.Id = @Id AND tplan.ProductId = @ProductId AND tplan.IsDeleted = 'False';

                    SELECT tplan.Id as TestPlanId, 
                    tsuite.Id as TestSuiteId,
                    tsuite.Name as TestSuiteTitle
                    FROM producttest.TestPlans tplan
                    INNER JOIN producttest.TestSuites tsuite ON tplan.Id = tsuite.TestPlanId
                    WHERE tplan.Id = @Id AND tplan.ProductId = @ProductId AND tSuite.IsDeleted = 'False';

                    SELECT tsuite.Id as TestSuiteId,
                    tcase.Id as TestCaseId,
                    tcase.Title as TestCaseTitle,
                    tcase.Preconditions
                    FROM producttest.TestPlans tplan
                    INNER JOIN producttest.TestSuites tsuite ON tplan.Id = tsuite.TestPlanId
                    INNER JOIN producttest.TestSuiteTestCaseMapping tstcm ON tstcm.TestSuiteId = tsuite.Id
                    INNER JOIN producttest.TestCases tcase ON tcase.Id = tstcm.TestCaseId
                    WHERE tplan.Id = @Id AND tplan.ProductId = @ProductId AND tcase.IsDeleted = 'False';

                    SELECT tcase.Id as TestCaseId,
                    tstep.Id as TestStepId,
                    tstep.StepNo, tstep.Action, tstep.ExpectedResult
                    FROM producttest.TestPlans tplan
                    INNER JOIN producttest.TestSuites tsuite ON tplan.Id = tsuite.TestPlanId
                    INNER JOIN producttest.TestSuiteTestCaseMapping tstcm ON tstcm.TestSuiteId = tsuite.Id
                    INNER JOIN producttest.TestCases tcase ON tcase.Id = tstcm.TestCaseId
                    INNER JOIN producttest.TestSteps tstep ON tstep.TestCaseId = tcase.Id
                    WHERE tplan.Id = @Id AND tplan.ProductId = @ProductId AND tstep.IsDeleted = 'False';";

                using (IDbConnection con = new SqlConnection(_queriesConnectionString))
                {
                    var result = await con.QueryMultipleAsync(sql, new
                    {
                        request.ProductId,
                        request.Id
                    });
                    testPlanDetails = (await result.ReadAsync<GetTestPlanDetailsDto>()).SingleOrDefault();
                    testPlanDetails.TestSuites = (await result.ReadAsync<TestSuiteDetailsDto>()).ToList();
                    var allTestCases = await result.ReadAsync<TestCaseDetailsDto>();
                    var allTestSteps = await result.ReadAsync<TestStepDetailsDto>();
                    foreach(TestSuiteDetailsDto testSuite in testPlanDetails.TestSuites)
                    {
                        testSuite.TestCases = allTestCases.Where(testcase => testcase.TestSuiteId == testSuite.TestSuiteId).ToList();
                        foreach(TestCaseDetailsDto testcase in testSuite.TestCases)
                        {
                            testcase.TestSteps = allTestSteps.Where(teststep => teststep.TestCaseId == testcase.TestCaseId).ToList();
                            testcase.TestSteps.Sort((x, y) => x.StepNo < y.StepNo ? -1 : 1);
                        }
                    }
                }
                return testPlanDetails;
            }
        }
    }
}
