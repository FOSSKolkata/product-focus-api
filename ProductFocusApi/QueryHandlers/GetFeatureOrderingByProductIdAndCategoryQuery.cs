using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using ProductFocus.ConnectionString;
using ProductFocusApi.Dtos;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;

namespace ProductFocusApi.QueryHandlers
{
    public sealed class GetFeatureOrderingByProductIdAndCategoryQuery : IRequest<List<FeatureOrderDto>>
    {
        public long ProductId { get; set; }
        public long SprintId { get; set; }
        public GetFeatureOrderingByProductIdAndCategoryQuery(long productId, long sprintId)
        {
            ProductId = productId;
            SprintId = sprintId;
        }
        internal sealed class GetFeatureOrderingByProductIdAndCategoryQueryHandler : IRequestHandler<GetFeatureOrderingByProductIdAndCategoryQuery, List<FeatureOrderDto>>
        {
            private readonly QueriesConnectionString _queriesConnectionString;
            public GetFeatureOrderingByProductIdAndCategoryQueryHandler(QueriesConnectionString queriesConnectionString)
            {
                _queriesConnectionString = queriesConnectionString;
            }
            public async System.Threading.Tasks.Task<List<FeatureOrderDto>> Handle(GetFeatureOrderingByProductIdAndCategoryQuery query, CancellationToken cancellationToken)
            {
                List<FeatureOrderDto> featureOrders = new();

                var sql = @"SELECT FeatureId, OrderNumber FROM FeatureOrderings fo
                            INNER JOIN Features f ON f.id = fo.featureId
                            INNER JOIN Modules m ON m.Id = f.ModuleId
                            INNER JOIN Products p ON m.ProductId=p.Id
                            WHERE fo.OrderingCategory = @OrderingCategory
                            AND fo.sprintId = @SprintId";

                using (IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    featureOrders = (await con.QueryAsync<FeatureOrderDto>(sql, new {
                        query.ProductId,
                        query.SprintId
                    })).ToList();
                }
                return featureOrders;
            }
        }
    }
}
