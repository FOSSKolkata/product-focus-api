using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Dapper;
using Releases.Domain.Model;

namespace Releases.Application.QueryHandler
{
    public sealed class GetReleasesQuery : IRequest<List<GetReleaseDto>>
    {
        public long ProductId { get; private set; }
        public GetReleasesQuery(long productId)
        {
            ProductId = productId;
        }
        internal class GetReleasesQueryHandler : IRequestHandler<GetReleasesQuery, List<GetReleaseDto>>
        {
            private readonly string _queryConnectionString;
            public GetReleasesQueryHandler(IConfiguration configuration)
            {
                _queryConnectionString = configuration["ConnectionStrings:QueriesConnectionString"] ?? throw new NullReferenceException("Connection String is missing from config file");
            }
            public async Task<List<GetReleaseDto>> Handle(GetReleasesQuery request, CancellationToken cancellationToken)
            {
                List<GetReleaseDto> releases = null;
                /*string sql = @"SELECT id, name, releaseDate FROM [release].[Releases] WHERE productId = @ProductId";
                using IDbConnection con = new SqlConnection(_queryConnectionString);
                releases = (await con.QueryAsync<GetReleaseDto>(sql, new
                {
                    request.ProductId
                })).ToList();
                return releases;*/
                string sql = @"SELECT id, Name, ReleaseDate, CreatedOn, Status FROM [release].[Releases]
                    WHERE productId = @ProductId;
                    SELECT id, releaseId, WorkItemType, WorkItemCount FROM
                    [release].[ReleaseWorkItemCounts] WHERE
                    releaseId IN (SELECT id FROM [release].[Releases] WHERE productId = @ProductId);";

                using(IDbConnection con = new SqlConnection(_queryConnectionString))
                {
                    var result = await con.QueryMultipleAsync(sql, new
                    {
                        request.ProductId
                    });

                    var tempReleases = await result.ReadAsync<GetReleaseDto>();
                    var tempReleaseWorkItemCount = await result.ReadAsync<GetReleaseWorkItemCountDto>();
                    foreach(var release in tempReleases)
                    {
                        var featureCount = tempReleaseWorkItemCount.Where(workItem => workItem.ReleaseId == release.Id && workItem.WorkItemType == WorkItemType.Feature).SingleOrDefault();
                        if(featureCount != null)
                        {
                            release.FeatureCount = featureCount.WorkItemCount;
                        }
                        var bugCount = tempReleaseWorkItemCount.Where(workItem => workItem.ReleaseId == release.Id && workItem.WorkItemType == WorkItemType.Bug).SingleOrDefault();
                        if(bugCount != null)
                        {
                            release.BugCount = bugCount.WorkItemCount;
                        }
                        var epicCount = tempReleaseWorkItemCount.Where(workItem => workItem.ReleaseId == release.Id && workItem.WorkItemType == WorkItemType.Epic).SingleOrDefault();
                        if(epicCount != null)
                        {
                            release.EpicCount = epicCount.WorkItemCount;
                        }
                        var pbiCount = tempReleaseWorkItemCount.Where(workItem => workItem.ReleaseId == release.Id && workItem.WorkItemType == WorkItemType.PBI).SingleOrDefault();
                        if(pbiCount != null)
                        {
                            release.PbiCount = pbiCount.WorkItemCount;
                        }
                    }
                    releases = tempReleases.ToList();
                }
                return releases;
            }
        }
    }
}
