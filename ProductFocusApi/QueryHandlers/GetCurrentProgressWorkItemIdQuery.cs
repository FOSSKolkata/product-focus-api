using Dapper;
using MediatR;
using ProductFocus.ConnectionString;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using ProductFocusApi.Dtos;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProductFocusApi.QueryHandlers
{
    public class GetCurrentProgressWorkItemIdQuery : IRequest<GetCurrentProgressWorkItemDto>
    {
        public long ProductId { get; private set; }
        public string UserId { get; private set; }
        public GetCurrentProgressWorkItemIdQuery(long productId, string userId)
        {
            ProductId = productId;
            UserId = userId;
        }
        internal class GetCurrentProgressWorkItemIdQueryHandler : IRequestHandler<GetCurrentProgressWorkItemIdQuery, GetCurrentProgressWorkItemDto>
        {
            private readonly QueriesConnectionString _queriesConnectionString;
            private readonly IUserRepository _userRepository;
            public GetCurrentProgressWorkItemIdQueryHandler(QueriesConnectionString queriesConnectionString, IUserRepository userRepository)
            {
                _queriesConnectionString = queriesConnectionString;
                _userRepository = userRepository;
            }

            public async Task<GetCurrentProgressWorkItemDto> Handle(GetCurrentProgressWorkItemIdQuery request, CancellationToken cancellationToken)
            {
                GetCurrentProgressWorkItemDto dto;
                User user = _userRepository.GetByIdpUserId(request.UserId);

                string sql = @"SELECT cpwi.id, cpwi.workItemId FROM [dbo].[CurrentProgressWorkItems] cpwi
                    INNER JOIN [dbo].[Features] f ON cpwi.WorkItemId = f.Id
                    WHERE cpwi.isDeleted = 'false' AND cpwi.productId = @ProductId AND cpwi.userId = @UserId
                    AND f.SprintId = (SELECT TOP(1) id FROM SPRINT WHERE productId = 4
                    AND startDate <= GETDATE() AND GETDATE() <= endDate
                    ORDER BY endDate DESC);";

                using(IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    dto = (await con.QueryAsync<GetCurrentProgressWorkItemDto>(sql, new
                    {
                        request.ProductId,
                        UserId = user.Id
                    })).FirstOrDefault();
                }
                return dto;
            }
        }
    }
}
