using Dapper;
using MediatR;
using ProductFocus.ConnectionString;
using ProductFocusApi.Dtos;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ProductFocusApi.QueryHandlers
{
    public sealed class GetFeatureListByProductIdQuery : IRequest<List<GetFeatureDto>>
    {
        public long ProductId { get; private set; }
        public GetFeatureListByProductIdQuery(long productId)
        {
            ProductId = productId;
        }
        public sealed class GetFeatureListByProductIdQueryHandler : IRequestHandler<GetFeatureListByProductIdQuery, List<GetFeatureDto>>
        {
            List<GetFeatureDto> features;
            private readonly QueriesConnectionString _queriesConnectionString;
            public GetFeatureListByProductIdQueryHandler(QueriesConnectionString queriesConnectionString)
            {
                _queriesConnectionString = queriesConnectionString;
            }
            public async Task<List<GetFeatureDto>> Handle(GetFeatureListByProductIdQuery request, CancellationToken cancellationToken)
            {
                string sql = @"SELECT id, title, workItemType FROM Features WHERE productId = @ProductId;";

                using(IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    features = (await con.QueryAsync<GetFeatureDto>(sql, new
                    {
                        request.ProductId
                    })).ToList();
                }
                return features;
            }
        }
    }
}
