using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Dapper;

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
                string sql = @"SELECT id, name, releaseDate FROM [release].[Releases] WHERE productId = @ProductId";
                using IDbConnection con = new SqlConnection(_queryConnectionString);
                releases = (await con.QueryAsync<GetReleaseDto>(sql, new
                {
                    request.ProductId
                })).ToList();
                return releases;
            }
        }
    }
}
