using CSharpFunctionalExtensions;
using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using ProductTests.Domain.Model.TestPlanAggregate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProductTests.Application.QueryHandler.GetTestResultQueries
{
    public class GetTestResultsQuery : IRequest<List<GetTestResultsDto>>
    {
        public long ProductId { get; private set; }
        public string SearchTitle { get; private set; }
        public List<TestTypeEnum> SearchTestTypes { get; private set; }
        public GetTestResultsQuery(long productId, string searchTitle, List<TestTypeEnum> searchTestTypes)
        {
            ProductId = productId;
            SearchTitle = searchTitle;
            SearchTestTypes = searchTestTypes;
        }
        internal class GetTestResultsQueryHandler : IRequestHandler<GetTestResultsQuery, List<GetTestResultsDto>>
        {
            private readonly string _queriesConnectionString;
            public GetTestResultsQueryHandler(IConfiguration configuration)
            {
                _queriesConnectionString = configuration["ConnectionStrings:QueriesConnectionString"] ?? throw new NullReferenceException("Connection String is missing from config file");
            }
            public async Task<List<GetTestResultsDto>> Handle(GetTestResultsQuery request, CancellationToken cancellationToken)
            {
                List<GetTestResultsDto> testResults = new();
                /*string sql = @"SELECT tsuite.id AS SuiteId, tcase.ResultStatus, COUNT(ResultStatus) AS ResultCount FROM [producttest].[TestPlansVersion] tplan
                        INNER JOIN [producttest].[TestSuitesVersion] tsuite ON tsuite.TestPlanVersionId = tplan.Id
                        INNER JOIN [producttest].[TestSuiteTestCaseMappingsVersion] tpsmap ON tsuite.Id = tpsmap.TestSuiteVersionId
                        INNER JOIN [producttest].[TestCasesVersion] tcase ON tcase.Id = tpsmap.TestCaseVersionId
                        WHERE tplan.productId = @ProductId AND (tsuite.Name LIKE '%test%' AND tplan.TestType in (0,1,2,3))
                        GROUP BY tsuite.Id, tcase.ResultStatus, tsuite.Name;

                        SELECT tplan.Id, tplan.Name AS Title, tsuite.Id AS TestSuiteId FROM [producttest].[TestPlansVersion] tplan
                        INNER JOIN [producttest].[TestSuitesVersion] AS tsuite ON tplan.Id = tsuite.TestPlanVersionId
                        WHERE tplan.ProductId = 4 AND (tsuite.Name LIKE '%test%' AND tplan.TestType in (0,1,2,3));";*/

                var builder1 = new SqlBuilder();
                var selector1 = builder1.AddTemplate(@"SELECT tsuite.id AS SuiteId, tcase.ResultStatus, COUNT(ResultStatus) AS ResultCount
                    FROM [producttest].[TestPlansVersion] tplan /**innerjoin**/ /**innerjoin**/ /**innerjoin**/ /**where**/ /**groupby**/");
                builder1.InnerJoin("[producttest].[TestSuitesVersion] tsuite ON tsuite.TestPlanVersionId = tplan.Id");
                builder1.InnerJoin("[producttest].[TestSuiteTestCaseMappingsVersion] tpsmap ON tsuite.Id = tpsmap.TestSuiteVersionId");
                builder1.InnerJoin("[producttest].[TestCasesVersion] tcase ON tcase.Id = tpsmap.TestCaseVersionId");
                builder1.Where("tplan.productId = @ProductId");

                if (!string.IsNullOrEmpty(request.SearchTitle))
                    builder1.Where("AND tsuite.Name LIKE '%@SearchTitle%'");
                if (request.SearchTestTypes.Count > 0)
                    builder1.Where("AND tplan.TestType in @SearchTestTypes");
                builder1.GroupBy("tsuite.Id, tcase.ResultStatus, tsuite.Name");
                var sql1 = selector1.RawSql;

                var builder2 = new SqlBuilder();
                var selector2 = builder2.AddTemplate(@"SELECT tplan.Id, tplan.Name AS Title, tsuite.Id AS TestSuiteId
                    FROM [producttest].[TestPlansVersion] tplan /**innerjoin**/ /**where**/");
                builder2.InnerJoin("[producttest].[TestSuitesVersion] AS tsuite ON tplan.Id = tsuite.TestPlanVersionId");
                builder2.Where("tplan.ProductId = @ProductId");
                if (!string.IsNullOrEmpty(request.SearchTitle))
                    builder2.Where("AND tsuite.Name LIKE '%@SearchTitle%'");
                if (request.SearchTestTypes.Count > 0)
                    builder2.Where("AND tplan.TestType in @SearchTestTypes");
                var sql2 = selector2.RawSql;

                using(IDbConnection con = new SqlConnection(_queriesConnectionString))
                {
                    var tempResultStatusAndCount = await con.QueryAsync<GetTestSuiteResultDto>(sql1, new
                    {
                        request.ProductId,
                        request.SearchTitle,
                        request.SearchTestTypes
                    });

                    var tempTestResults = await con.QueryAsync<GetTestResultsDto>(sql2, new
                    {
                        request.ProductId,
                        request.SearchTitle,
                        request.SearchTestTypes
                    });

                }
                return testResults;
            }
        }
    }
}
