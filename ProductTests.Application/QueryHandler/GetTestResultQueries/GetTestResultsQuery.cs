using CSharpFunctionalExtensions;
using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using ProductTests.Domain.Model.TestPlanAggregate;
using ProductTests.Domain.Model.TestRunAggregate;
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
    public class GetTestResultsQuery : IRequest<List<GetTestResultDto>>
    {
        public long TestPlanId { get; private set; }
        public string SearchTitle { get; private set; }
        public List<TestTypeEnum> SearchTestTypes { get; private set; }
        public GetTestResultsQuery(long testPlanId, string searchTitle, List<TestTypeEnum> searchTestTypes)
        {
            TestPlanId = testPlanId;
            SearchTitle = searchTitle;
            SearchTestTypes = searchTestTypes;
        }
        internal class GetTestResultsQueryHandler : IRequestHandler<GetTestResultsQuery, List<GetTestResultDto>>
        {
            private readonly string _queriesConnectionString;
            public GetTestResultsQueryHandler(IConfiguration configuration)
            {
                _queriesConnectionString = configuration["ConnectionStrings:QueriesConnectionString"] ?? throw new NullReferenceException("Connection String is missing from config file");
            }
            public async Task<List<GetTestResultDto>> Handle(GetTestResultsQuery request, CancellationToken cancellationToken)
            {
                List<GetTestResultDto> testResults = new();
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
                var selector1 = builder1.AddTemplate(@"SELECT trun.Id AS TestRunId, trun.SprintId, trun.RunningStatus,
                    CONCAT(tplan.Name,'_', DATENAME(MICROSECOND, trun.CreatedOn)) AS Title, tplan.TestType,
                    trun.CreatedOn, tplan.Id AS TestPlanVersionId FROM [producttest].[TestRuns] trun /**innerjoin**/ /**where**/");
                builder1.InnerJoin("[producttest].[TestPlansVersion] tplan ON trun.TestPlanVersionId = tplan.Id");
                builder1.Where("trun.TestPlanId = @TestPlanId");
                if(!string.IsNullOrEmpty(request.SearchTitle))
                {
                    builder1.Where($"CONCAT(tplan.Name,'_', DATENAME(MICROSECOND, trun.CreatedOn)) LIKE '%{request.SearchTitle}%'");
                }
                if(request.SearchTestTypes.Count > 0)
                {
                    builder1.Where("tplan.TestType in @SearchTestTypes");
                }
                var sql1 = selector1.RawSql;

                var builder2 = new SqlBuilder();
                var selector2 = builder2.AddTemplate(@"SELECT trun.id AS TestRunId, tplan.Id AS TestPlanId,
                    tsuite.Id AS TestSuiteId, tcase.ResultStatus FROM [producttest].[TestRuns] trun /**innerjoin**/ /**where**/ /**groupby**/");

                builder2.InnerJoin("[producttest].[TestPlansVersion] tplan ON tplan.Id = trun.TestPlanVersionId");
                builder2.InnerJoin("[producttest].[TestSuitesVersion] tsuite ON tplan.Id = tsuite.TestPlanVersionId");
                builder2.InnerJoin("[producttest].[TestCasesVersion] tcase ON tcase.TestSuiteVersionId = tsuite.Id");
                builder2.Where("trun.TestPlanId = @TestPlanId");

                if (!string.IsNullOrEmpty(request.SearchTitle))
                    builder2.Where($"CONCAT(tplan.Name,'_', DATENAME(MICROSECOND, trun.CreatedOn)) LIKE '%{request.SearchTitle}%'");
                if (request.SearchTestTypes.Count > 0)
                    builder2.Where("tplan.TestType in @SearchTestTypes");
                builder1.GroupBy("trun.id, tplan.Id, trun.CreatedOn, tsuite.Id, tcase.ResultStatus;");
                var sql2 = selector2.RawSql;

                using(IDbConnection con = new SqlConnection(_queriesConnectionString))
                {
                    var tempTestResults = await con.QueryAsync<GetTestResultDto>(sql1, new
                    {
                        request.TestPlanId,
                        request.SearchTitle,
                        request.SearchTestTypes
                    });

                    var tempTestResultsDetails = await con.QueryAsync<GetTestResultDetailsDto>(sql2, new
                    {
                        request.TestPlanId,
                        request.SearchTitle,
                        request.SearchTestTypes
                    });

                    foreach(var result in tempTestResults)
                    {
                        result.Passed = tempTestResultsDetails.Where(x => x.TestRunId == result.TestRunId && x.ResultStatus == TestCaseResult.Success).Count();
                        result.Failed = tempTestResultsDetails.Where(x => x.TestRunId == result.TestRunId && x.ResultStatus == TestCaseResult.Failed).Count();
                    }
                    testResults = tempTestResults.ToList();
                }
                return testResults;
            }
        }
    }
}
