using ProductFocus.Domain;
using ProductFocus.Dtos;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using ProductFocus.ConnectionString;
using ProductFocus.Services;
using System.Threading.Tasks;

namespace ProductFocus.AppServices
{
    public sealed class GetUserListNotInOrganizationQuery : IQuery<List<GetUserNotPartOfOrgDto>>
    {
        public long Id { get; }
        public GetUserListNotInOrganizationQuery(long id)
        {
            Id = id;
        }

        internal sealed class GetUserListNotInOrganizationQueryHandler : IQueryHandler<GetUserListNotInOrganizationQuery, List<GetUserNotPartOfOrgDto>>
        {
            private readonly QueriesConnectionString _queriesConnectionString;
            private readonly IEmailService _emailService;

            public GetUserListNotInOrganizationQueryHandler(QueriesConnectionString queriesConnectionString, IEmailService emailService)
            {
                _queriesConnectionString = queriesConnectionString;
                _emailService = emailService;
            }
            public async Task<List<GetUserNotPartOfOrgDto>> Handle(GetUserListNotInOrganizationQuery query)
            {
                List<GetUserNotPartOfOrgDto> userList = new List<GetUserNotPartOfOrgDto>();
                
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
                
                //_emailService.send();
                
                return userList;
            }
        }
    }
}
