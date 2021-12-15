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
    public class GetTagCategoryListQuery : IQuery<List<GetTagCategoryDto>>
    {
        public long Id { get; set; }
        public GetTagCategoryListQuery(long id)
        {
            Id = id;
        }
        internal sealed class GetTagCategoryListQueryHandler : IQueryHandler<GetTagCategoryListQuery, List<GetTagCategoryDto>>
        {
            private readonly QueriesConnectionString _queriesConnectionString;
            public GetTagCategoryListQueryHandler(QueriesConnectionString queriesConnectionString)
            {
                _queriesConnectionString = queriesConnectionString;
            }
            public async Task<List<GetTagCategoryDto>> Handle(GetTagCategoryListQuery query)
            {
                List<GetTagCategoryDto> tagCategoryList = new();
                string sql = @"SELECT Id, Name from TagCategory WHERE ProductId = @ProductId";

                using (IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    tagCategoryList = (await con.QueryAsync<GetTagCategoryDto>(sql, new
                    {
                        ProductId = query.Id
                    })).ToList();
                }
                return tagCategoryList;
            }
        }
    }
}
