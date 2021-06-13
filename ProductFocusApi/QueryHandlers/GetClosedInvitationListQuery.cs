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
    public sealed class GetClosedInvitationListQuery : IQuery<GetClosedInvitationDto>
    {
        public long OrgId { get; set; }
        public int Offset { get; set; }
        public int Count { get; set; }
        public GetClosedInvitationListQuery(long orgId, int offset, int count)
        {
            OrgId = orgId;
            Offset = offset;
            Count = count;
        }

        internal sealed class GetClosedInvitationListQueryHandler : IQueryHandler<GetClosedInvitationListQuery, GetClosedInvitationDto>
        {
            private readonly QueriesConnectionString _queriesConnectionString;            

            public GetClosedInvitationListQueryHandler(QueriesConnectionString queriesConnectionString)
            {
                _queriesConnectionString = queriesConnectionString;                
            }
            public async Task<GetClosedInvitationDto> Handle(GetClosedInvitationListQuery query)
            {
                GetClosedInvitationDto closedInvitationList = new GetClosedInvitationDto();
                
                string sql = @"
                    select count(1) as RecordCount
                    from Invitations
                    where Status in (2,3,4)
                    and OrganizationId = @OrgId
                    ;
                    select Id, Email, OrganizationId, InvitedOn, ActionedOn, Status 
                    from Invitations
                    where Status in (2,3,4)
                    and OrganizationId = @OrgId
                    order by Id desc
                    offset @Offset rows
                    fetch next @Count rows only";
                
                using (IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    var result = await con.QueryMultipleAsync(sql, new
                    {
                        OrgId = query.OrgId,
                        Offset = query.Offset,
                        Count = query.Count
                    });

                    var closedInvitations = await result.ReadAsync<GetClosedInvitationDto>();
                    var invitationDetails = await result.ReadAsync<ClosedInvitationDetails>();

                    closedInvitations.SingleOrDefault().ClosedInvitations = invitationDetails.ToList();

                    closedInvitationList = closedInvitations.SingleOrDefault();
                }
                
                return closedInvitationList;
            }
        }
    }
}
