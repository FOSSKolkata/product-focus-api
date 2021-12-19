using Dapper;
using Microsoft.Data.SqlClient;
using ProductFocus.ConnectionString;
using ProductFocus.Domain;
using ProductFocusApi.Dtos;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ProductFocusApi.QueryHandlers
{
    public sealed class GetTagListQuery : IQuery<List<GetTagDto>>
    {
        public long ProductId { get; private set; }
        public GetTagListQuery(long productId)
        {
            ProductId = productId;
        }
        internal sealed class GetTagListQueryHandler : IQueryHandler<GetTagListQuery, List<GetTagDto>>
        {
            private readonly QueriesConnectionString _queriesConnectionString;
            public GetTagListQueryHandler(QueriesConnectionString queriesConnectionString)
            {
                _queriesConnectionString = queriesConnectionString;
            }
            public async Task<List<GetTagDto>> Handle(GetTagListQuery query)
            {
                List<GetTagDto> tagList = new();

                string sql = @"SELECT Id, Name from Tags Where ProductId = @ProductId";

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
