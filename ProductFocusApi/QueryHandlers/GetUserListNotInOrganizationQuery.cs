using ProductFocus.Dtos;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using ProductFocus.ConnectionString;
using System.Threading.Tasks;
using MediatR;
using System.Threading;

namespace ProductFocus.AppServices
{
    public sealed class GetUserListNotInOrganizationQuery : IRequest<List<GetUserNotPartOfOrgDto>>
    {
        public long Id { get; }
        public GetUserListNotInOrganizationQuery(long id)
        {
            Id = id;
        }

        internal sealed class GetUserListNotInOrganizationQueryHandler : IRequestHandler<GetUserListNotInOrganizationQuery, List<GetUserNotPartOfOrgDto>>
        {
            private readonly QueriesConnectionString _queriesConnectionString;

            public GetUserListNotInOrganizationQueryHandler(QueriesConnectionString queriesConnectionString)
            {
                _queriesConnectionString = queriesConnectionString;
            }
            public async Task<List<GetUserNotPartOfOrgDto>> Handle(GetUserListNotInOrganizationQuery query, CancellationToken cancellationToken)
            {
                List<GetUserNotPartOfOrgDto> userList = new();
                
                string sql = @"
                    select u.Id, u.Name, u.Email from Users u
                    where u.Id not in (
                    select m.UserId from Organizations o, Members m
                    where o.Id = m.OrganizationId
                    and o.Id = @OrgId)";
                
                using (IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    userList = (await con.QueryAsync<GetUserNotPartOfOrgDto>(sql, new
                    {
                        OrgId = query.Id
                    })).ToList();
                }
                
                return userList;
            }
        }
    }
}
