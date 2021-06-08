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
    public sealed class GetPendingInvitationListQuery : IQuery<List<GetPendingInvitationDto>>
    {
        public long OrgId { get; set; }
        public int Offset { get; set; }
        public int Count { get; set; }
        public GetPendingInvitationListQuery(long orgId, int offset, int count)
        {
            OrgId = orgId;
            Offset = offset;
            Count = count;
        }

        internal sealed class GetPendingInvitationListQueryHandler : IQueryHandler<GetPendingInvitationListQuery, List<GetPendingInvitationDto>>
        {
            private readonly QueriesConnectionString _queriesConnectionString;            

            public GetPendingInvitationListQueryHandler(QueriesConnectionString queriesConnectionString)
            {
                _queriesConnectionString = queriesConnectionString;                
            }
            public async Task<List<GetPendingInvitationDto>> Handle(GetPendingInvitationListQuery query)
            {
                List<GetPendingInvitationDto> pendingInvitationList = new List<GetPendingInvitationDto>();
                
                string sql = @"
                    select Id, Email, OrganizationId, InvitedOn, LastResentOn, Status 
                    from Invitations
                    where Status in (1,5)
                    and OrganizationId = @OrgId
                    order by Id desc
                    offset @Offset rows
                    fetch next @Count rows only";
                
                using (IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    pendingInvitationList = (await con.QueryAsync<GetPendingInvitationDto>(sql, new
                    {
                        OrgId = query.OrgId,
                        Offset = query.Offset,
                        Count = query.Count
                    })).ToList();
                }
                
                return pendingInvitationList;
            }
        }
    }
}
