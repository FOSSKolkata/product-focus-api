﻿using ProductFocus.Dtos;
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
    public sealed class GetClosedInvitationListQuery : IRequest<GetClosedInvitationDto>
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

        internal sealed class GetClosedInvitationListQueryHandler : IRequestHandler<GetClosedInvitationListQuery, GetClosedInvitationDto>
        {
            private readonly QueriesConnectionString _queriesConnectionString;            

            public GetClosedInvitationListQueryHandler(QueriesConnectionString queriesConnectionString)
            {
                _queriesConnectionString = queriesConnectionString;                
            }
            public async Task<GetClosedInvitationDto> Handle(GetClosedInvitationListQuery query, CancellationToken cancellationToken)
            {
                GetClosedInvitationDto closedInvitationList = new();
                
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
                        query.OrgId,
                        query.Offset,
                        query.Count
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
