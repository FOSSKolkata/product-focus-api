using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProductTests.Application.QueryHandler.GetTestPlanQueries
{
    public sealed class GetTestPlansQuery : IRequest<List<GetTestPlansDto>>
    {
        public long ProductId { get; private set; }
        public GetTestPlansQuery(long productId)
        {
            ProductId = productId;
        }

        internal sealed class GetTestPlansQueryHandler : IRequestHandler<GetTestPlansQuery, List<GetTestPlansDto>>
        {
            private readonly string _queriesConnectionString;
            public GetTestPlansQueryHandler(IConfiguration configuration)
            {
                _queriesConnectionString = configuration["ConnectionStrings:QueriesConnectionString"] ?? throw new NullReferenceException("Connection String is missing from config file");
            }

            public async Task<List<GetTestPlansDto>> Handle(GetTestPlansQuery request, CancellationToken cancellationToken)
            {
                List<GetTestPlansDto> testPlans = new();
                //string sql = @"SELECT tp.Id, tp.Name as Title, tp.sprintId, COUNT(*) as 'suiteCount', tp.TestType FROM producttest.TestPlans tp
                //    LEFT JOIN producttest.TestSuites ts
                //    ON tp.Id = ts.TestPlanId
                //    WHERE tp.IsDeleted = 'False' AND ts.IsDeleted = 'False' AND tp.ProductId = @ProductId
                //    GROUP BY tp.Id, tp.Name, tp.SprintId, tp.TestType; ";
                string sql = @"SELECT tp.Id, tp.Name as Title, sp.Name as SprintTitle, tp.TestType FROM producttest.TestPlans tp
                                LEFT JOIN dbo.sprint sp ON sp.Id = tp.SprintId WHERE tp.IsDeleted = 'False' AND tp.ProductId = @ProductId;
                    SELECT ts.Id, ts.TestPlanId FROM producttest.TestSuites ts;";

                using (IDbConnection con = new SqlConnection(_queriesConnectionString))
                {
                    //testPlans = (await con.QueryAsync<GetTestPlansDto>(sql, new
                    //{
                    //    request.ProductId
                    //})).ToList();
                    var result = await con.QueryMultipleAsync(sql, new
                    {
                        request.ProductId
                    });
                    var testPlansEnumerable = await result.ReadAsync<GetTestPlansDto>();
                    var testSuitesEnumerable = await result.ReadAsync<GetTestPlanSuiteIdDto>();
                    foreach(var testPlan in testPlansEnumerable)
                    {
                        testPlan.SuiteCount = testSuitesEnumerable.Where(testsuite => testsuite.TestPlanId == testPlan.Id).Count();
                    }
                    testPlans = testPlansEnumerable.ToList();
                }
                return testPlans;
            }
        }
    }
}
