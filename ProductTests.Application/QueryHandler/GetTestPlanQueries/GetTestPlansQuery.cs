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
                string sql = @"SELECT Id, Name AS Title, SprintId, TestType FROM producttest.TestPlans WHERE productId = @ProductId AND IsDeleted = 'False';";
                string suiteCountSql = @"
                    SELECT tp.Id, ts.Id as SuiteId FROM producttest.TestPlans tp
                    INNER JOIN producttest.TestSuites ts ON tp.Id = ts.TestPlanId WHERE tp.IsDeleted = 'False' AND ts.IsDeleted = 'False';";
                using(IDbConnection con = new SqlConnection(_queriesConnectionString))
                {
                    testPlans = (await con.QueryAsync<GetTestPlansDto>(sql, new
                    {
                        request.ProductId
                    })).ToList();

                    var allTestSuites = await con.QueryAsync<GetTestPlanWithSuiteIdDto>(suiteCountSql, new
                    {

                    });
                    foreach(var testPlan in testPlans)
                    {
                       testPlan.SuiteCount = allTestSuites.Where(x => x.Id == testPlan.Id).Count();
                    }
                }
                return testPlans;
            }
        }
    }
}
