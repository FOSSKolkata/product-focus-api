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
    public sealed class GetOrganizationListByUserQuery : IQuery<List<GetOrganizationByUserDto>>
    {
        public long UserId { get; }
        public GetOrganizationListByUserQuery(long userId)
        {
            UserId = userId;
        }

        internal sealed class GetOrganizationListByUserQueryHandler : IQueryHandler<GetOrganizationListByUserQuery, List<GetOrganizationByUserDto>>
        {
            private readonly QueriesConnectionString _queriesConnectionString;
            private readonly IEmailService _emailService;

            public GetOrganizationListByUserQueryHandler(QueriesConnectionString queriesConnectionString, IEmailService emailService)
            {
                _queriesConnectionString = queriesConnectionString;
                _emailService = emailService;
            }
            public async Task<List<GetOrganizationByUserDto>> Handle(GetOrganizationListByUserQuery query)
            {
                List<GetOrganizationByUserDto> organizationList = new List<GetOrganizationByUserDto>();
                
                string sql = @"
                    select o.Id, o.Name, m.IsOwner 
                    from organizations o, members m
                    where o.id = m.organizationid
                    and userid = @UserId";
                
                using (IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    organizationList = (await con.QueryAsync<GetOrganizationByUserDto>(sql, new
                    {
                        UserId = query.UserId
                    })).ToList();
                }
                
                _emailService.send();
                
                return organizationList;
            }
        }
    }
}
