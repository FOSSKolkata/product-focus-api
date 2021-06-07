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
        public GetPendingInvitationListQuery()
        {
        
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
                    where Status in (1,5)";
                
                using (IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    pendingInvitationList = (await con.QueryAsync<GetPendingInvitationDto>(sql)).ToList();
                }
                
                return pendingInvitationList;
            }
        }
    }
}
