﻿using ProductFocus.Domain;
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
    public sealed class GetPendingInvitationListQuery : IQuery<GetPendingInvitationDto>
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

        internal sealed class GetPendingInvitationListQueryHandler : IQueryHandler<GetPendingInvitationListQuery, GetPendingInvitationDto>
        {
            private readonly QueriesConnectionString _queriesConnectionString;            

            public GetPendingInvitationListQueryHandler(QueriesConnectionString queriesConnectionString)
            {
                _queriesConnectionString = queriesConnectionString;                
            }
            public async Task<GetPendingInvitationDto> Handle(GetPendingInvitationListQuery query)
            {
                GetPendingInvitationDto pendingInvitationList = new GetPendingInvitationDto();
                
                string sql = @"
                    select count(1) as RecordCount
                    from Invitations
                    where Status in (1,5)
                    and OrganizationId = @OrgId
                    ;
                    select Id, Email, OrganizationId, InvitedOn, LastResentOn, Status 
                    from Invitations
                    where Status in (1,5)
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

                    var pendingInvitations = await result.ReadAsync<GetPendingInvitationDto>();
                    var invitationDetails = await result.ReadAsync<InvitationDetails>();

                    pendingInvitations.SingleOrDefault().PendingInvitations = invitationDetails.ToList();

                    pendingInvitationList = pendingInvitations.SingleOrDefault();
                }
                
                return pendingInvitationList;
            }
        }
    }
}
