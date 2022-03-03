using Dapper;
using Microsoft.Data.SqlClient;
using ProductFocus.ConnectionString;
using ProductFocusApi.Dtos;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using System.Threading;

namespace ProductFocusApi.QueryHandlers
{
    public sealed class GetTagListQuery : IRequest<List<GetTagDto>>
    {
        public long ProductId { get; private set; }
        public GetTagListQuery(long productId)
        {
            ProductId = productId;
        }
        internal sealed class GetTagListQueryHandler : IRequestHandler<GetTagListQuery, List<GetTagDto>>
        {
            private readonly QueriesConnectionString _queriesConnectionString;
            public GetTagListQueryHandler(QueriesConnectionString queriesConnectionString)
            {
                _queriesConnectionString = queriesConnectionString;
            }
            public async Task<List<GetTagDto>> Handle(GetTagListQuery query, CancellationToken cancellationToken)
            {
                List<GetTagDto> tagList = new();

                string sql = @"SELECT Id, Name FROM Tags WHERE ProductId = @ProductId AND IsDeleted = 'false' ORDER BY Name";

                using(IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    tagList = (await con.QueryAsync<GetTagDto>(sql, new
                    {
                        query.ProductId
                    })).ToList();
                }
                return tagList;
            }
        }
    }
}
