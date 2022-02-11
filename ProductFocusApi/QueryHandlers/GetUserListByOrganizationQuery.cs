using ProductFocus.Dtos;
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
    public sealed class GetUserListByOrganizationQuery : IRequest<GetMemberOfOrganizationDto>
    {
        public long OrgId { get; set; }
        
        public GetUserListByOrganizationQuery(long orgId)
        {
            OrgId = orgId;                        
        }

        internal sealed class GetUserListByOrganizationQueryHandler : IRequestHandler<GetUserListByOrganizationQuery, GetMemberOfOrganizationDto>
        {
            private readonly QueriesConnectionString _queriesConnectionString;            

            public GetUserListByOrganizationQueryHandler(QueriesConnectionString queriesConnectionString)
            {
                _queriesConnectionString = queriesConnectionString;                
            }
            public async Task<GetMemberOfOrganizationDto> Handle(GetUserListByOrganizationQuery query, CancellationToken cancellationToken)
            {
                GetMemberOfOrganizationDto userList = new();
                
                string sql = @"
                    select count(1) as RecordCount
                    from Organizations o, Members m
                    where o.Id = m.OrganizationId
                    and o.Id = @OrgId
                    ;
                    select u.Id, u.Name, u.Email, m.IsOwner from Organizations o, Members m, Users u
                    where o.Id = m.OrganizationId
                    and m.UserId = u.Id
                    and o.Id = @OrgId";
                
                using (IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    var result = await con.QueryMultipleAsync(sql, new
                    {
                        query.OrgId                        
                    });

                    var responseList = await result.ReadAsync<GetMemberOfOrganizationDto>();
                    var memberDetails = await result.ReadAsync<MemberDetails>();

                    responseList.SingleOrDefault().Members = memberDetails.ToList();

                    userList = responseList.SingleOrDefault();
                }
                
                return userList;
            }
        }
    }
}
