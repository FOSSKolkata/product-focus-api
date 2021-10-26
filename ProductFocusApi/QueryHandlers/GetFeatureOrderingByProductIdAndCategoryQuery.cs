using Dapper;
using Microsoft.Data.SqlClient;
using ProductFocus.ConnectionString;
using ProductFocus.Domain;
using ProductFocus.Domain.Model;
using ProductFocusApi.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ProductFocusApi.QueryHandlers
{
    public sealed class GetFeatureOrderingByProductIdAndCategoryQuery : IQuery<List<FeatureOrderDto>>
    {
        public long ProductId { get; set; }
        public long SprintId { get; set; }
        public OrderingCategoryEnum OrderingCategory { get; set; }
        public GetFeatureOrderingByProductIdAndCategoryQuery(long productId, long sprintId, OrderingCategoryEnum orderingCategory)
        {
            ProductId = productId;
            SprintId = sprintId;
            OrderingCategory = orderingCategory;
        }
        internal sealed class GetFeatureOrderingByProductIdAndCategoryQueryHandler : IQueryHandler<GetFeatureOrderingByProductIdAndCategoryQuery, List<FeatureOrderDto>>
        {
            private readonly QueriesConnectionString _queriesConnectionString;
            public GetFeatureOrderingByProductIdAndCategoryQueryHandler(QueriesConnectionString queriesConnectionString)
            {
                _queriesConnectionString = queriesConnectionString;
            }
            public async System.Threading.Tasks.Task<List<FeatureOrderDto>> Handle(GetFeatureOrderingByProductIdAndCategoryQuery query)
            {
                List<FeatureOrderDto> featureOrders = new List<FeatureOrderDto>();

                var sql = @"SELECT FeatureId, OrderNumber FROM FeatureOrderings fo
                            INNER JOIN Features f ON f.id = fo.featureId
                            INNER JOIN Modules m ON m.Id = f.ModuleId
                            INNER JOIN Products p ON m.ProductId=p.Id
                            WHERE fo.OrderingCategory = @OrderingCategory
                            AND fo.sprintId = @SprintId";

                using (IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    featureOrders = (await con.QueryAsync<FeatureOrderDto>(sql, new {
                        ProductId = query.ProductId,
                        SprintId = query.SprintId,
                        OrderingCategory = query.OrderingCategory
                    })).ToList();
                }
                return featureOrders;
            }
        }
    }
}
