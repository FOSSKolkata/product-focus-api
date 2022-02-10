﻿using ProductFocus.Domain.Common;
using ProductFocus.Dtos;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using ProductFocus.ConnectionString;
using System.Threading.Tasks;

namespace ProductFocus.AppServices
{
    public sealed class GetProductListQuery : IQuery<List<GetProductDto>>
    {
        public long Id { get; }
        public GetProductListQuery(long id)
        {
            Id = id;
        }

        internal sealed class GetProductListQueryHandler : IQueryHandler<GetProductListQuery, List<GetProductDto>>
        {
            private readonly QueriesConnectionString _queriesConnectionString;

            public GetProductListQueryHandler(QueriesConnectionString queriesConnectionString)
            {
                _queriesConnectionString = queriesConnectionString;
            }
            public async Task<List<GetProductDto>> Handle(GetProductListQuery query)
            {
                List<GetProductDto> productList = new();
                
                string sql = @"
                    SELECT id, name 
                    from [product-focus].[dbo].[Products]
                    WHERE organizationid = @OrgId";
                
                using (IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    productList = (await con.QueryAsync<GetProductDto>(sql, new
                    {
                        OrgId = query.Id
                    })).ToList();
                }
                
                return productList;
            }
        }
    }
}
