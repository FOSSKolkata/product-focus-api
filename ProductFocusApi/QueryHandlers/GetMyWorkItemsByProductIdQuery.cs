using Dapper;
using MediatR;
using ProductFocus.ConnectionString;
using ProductFocusApi.Dtos;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProductFocusApi.QueryHandlers
{
    public sealed class GetMyWorkItemsByProductIdQuery : IRequest<List<GetWorkItemDto>>
    {
        public long ProductId { get; private set; }
        public string UserObjectId { get; private set; }
        public GetMyWorkItemsByProductIdQuery(long productId, string userObjectId)
        {
            ProductId = productId;
            UserObjectId = userObjectId;
        }
        internal sealed class GetWorkItemsByProductIdQueryHandler : IRequestHandler<GetMyWorkItemsByProductIdQuery, List<GetWorkItemDto>>
        {
            private readonly QueriesConnectionString _queriesConnectionString;
            public GetWorkItemsByProductIdQueryHandler(QueriesConnectionString queriesConnectionString)
            {
                _queriesConnectionString = queriesConnectionString;
            }
            public async Task<List<GetWorkItemDto>> Handle(GetMyWorkItemsByProductIdQuery request, CancellationToken cancellationToken)
            {
                List<GetWorkItemDto> workItems = new();
                string sql = @"SELECT f.id, f.title, f.workItemType, f.description, f.workCompletionPercentage FROM [dbo].[Features] f
                    INNER JOIN [dbo].[UserToFeatureAssignments] fu ON f.Id = fu.FeatureId
                    INNER JOIN [dbo].[Users] u ON u.Id = fu.UserId
                    WHERE f.productId = @ProductId AND u.ObjectId = @UserObjectId;";

                using (IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    workItems = (await con.QueryAsync<GetWorkItemDto>(sql, new
                    {
                        request.ProductId,
                        request.UserObjectId,
                    })).ToList();
                }
                return workItems;
            }
        }
    }
}
