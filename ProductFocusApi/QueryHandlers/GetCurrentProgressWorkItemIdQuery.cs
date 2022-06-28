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

                string sql = @"SELECT id, workItemId FROM [dbo].[CurrentProgressWorkItems]
                    WHERE isDeleted = 'false' AND productId = @ProductId AND userId = @UserId;";

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
