﻿using Dapper;
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
    public sealed class GetTagCategoryListQuery : IRequest<List<GetTagCategoryDto>>
    {
        public long ProductId { get; set; }
        public GetTagCategoryListQuery(long productId)
        {
            ProductId = productId;
        }
        internal sealed class GetTagCategoryListQueryHandler : IRequestHandler<GetTagCategoryListQuery, List<GetTagCategoryDto>>
        {
            private readonly QueriesConnectionString _queriesConnectionString;

            public GetTagCategoryListQueryHandler(QueriesConnectionString queriesConnectionString)
            {
                _queriesConnectionString = queriesConnectionString;
            }
            public async Task<List<GetTagCategoryDto>> Handle(GetTagCategoryListQuery query, CancellationToken cancellationToken)
            {
                List<GetTagCategoryDto> tagCategoryList = new();

                string sql = @"
                    SELECT id, name
                    FROM [dbo].[TagCategories]
                    WHERE ProductId = @ProductId";

                using (IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    tagCategoryList = (await con.QueryAsync<GetTagCategoryDto>(sql, new
                    {
                        query.ProductId
                    })).ToList();
                }
                return tagCategoryList;
            }
        }
    }
}
