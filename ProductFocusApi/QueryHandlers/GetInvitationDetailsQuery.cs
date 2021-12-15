using CSharpFunctionalExtensions;
using Dapper;
using Microsoft.Data.SqlClient;
using ProductFocus.ConnectionString;
using ProductFocus.Domain;
using ProductFocus.Domain.Model;
using ProductFocusApi.Dtos;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ProductFocusApi.QueryHandlers
{
    public class GetInvitationDetailsQuery : IQuery<Result<GetInvitationDetailsDto>>
    {
        public long Id { get; set; }
        public string ObjectId { get; set; }
        public GetInvitationDetailsQuery(long id, string objectId)
        {
            Id = id;
            ObjectId = objectId;
        }
        internal sealed class GetInvitationDetailsQueryHandler : IQueryHandler<GetInvitationDetailsQuery, Result<GetInvitationDetailsDto>>
        {
            private readonly QueriesConnectionString _queriesConnectionString;
            public GetInvitationDetailsQueryHandler(QueriesConnectionString queriesConnectionString)
            {
                _queriesConnectionString = queriesConnectionString;
            }
            public async Task<Result<GetInvitationDetailsDto>> Handle(GetInvitationDetailsQuery query)
            {
                GetInvitationDetailsDto invitationDetails = new GetInvitationDetailsDto();
                string sql1 = @"SELECT Email FROM Users WHERE ObjectId = @ObjectId";
                string sql2 = @"SELECT Id, Email, OrganizationId, CreatedById, Status FROM Invitations WHERE Id = @Id;";
                string sql3 = @"SELECT * FROM Organizations WHERE Id = @OrgId;
                                SELECT * FROM Users WHERE Id = @UserId;";

                using(IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    string email = (await con.QueryAsync<string>(sql1, new
                    {
                        ObjectId = query.ObjectId
                    })).SingleOrDefault();

                    var invitation = (await con.QueryAsync<InvitationDetails>(sql2, new
                    { 
                        Id = query.Id
                    })).SingleOrDefault();

                    if (invitation == null)
                    {
                        return Result.Failure<GetInvitationDetailsDto>("Invalid Invitation.");
                    }

                    if (invitation.Email != email)
                    {
                        return Result.Failure<GetInvitationDetailsDto>("Access Denied.");
                    }

                    if (invitation.Status == (long)InvitationStatus.Accepted)
                    {
                        return Result.Failure<GetInvitationDetailsDto>("You are already a member.");
                    }

                    if(invitation.Status == (long)InvitationStatus.Cancelled || 
                        invitation.Status == (long)InvitationStatus.Rejected)
                    {
                        return Result.Failure<GetInvitationDetailsDto>("Sorry, this link is no longer valid.");
                    }

                    var result = (await con.QueryMultipleAsync(sql3, new
                    {
                        OrgId = invitation.OrganizationId,
                        UserId = invitation.CreatedById
                    }));

                    var organization = (await result.ReadAsync<Organization>()).Single();
                    var user = (await result.ReadAsync<User>()).Single();
                    invitationDetails.SenderName = user.Name;
                    invitationDetails.SenderEmail = user.Email;
                    invitationDetails.OrganizationName = organization.Name;
                }
                return Result.Success(invitationDetails);
            }
        }
    }
}
