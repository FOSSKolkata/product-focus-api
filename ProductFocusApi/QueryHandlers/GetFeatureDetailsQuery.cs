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
    public sealed class GetFeatureDetailsQuery : IRequest<GetFeatureDetailsDto>
    {
        public long OrgId { get; set; }
        public long Id { get; set; }
        
        public GetFeatureDetailsQuery(long orgId, long id)
        {
            Id = id;
            OrgId = orgId;
        }

        internal sealed class GetFeatureDetailsQueryHandler : IRequestHandler<GetFeatureDetailsQuery, GetFeatureDetailsDto>
        {
            private readonly QueriesConnectionString _queriesConnectionString;

            public GetFeatureDetailsQueryHandler(QueriesConnectionString queriesConnectionString)
            {
                _queriesConnectionString = queriesConnectionString;
            }
            public async Task<GetFeatureDetailsDto> Handle(GetFeatureDetailsQuery query, CancellationToken cancellationToken)
            {
                GetFeatureDetailsDto featureDetails = new();
                
                string sql = @"
                    select Id, Title, Description, WorkCompletionPercentage, 
                            Status, StoryPoint, IsBlocked,
                            AcceptanceCriteria, PlannedStartDate, PlannedEndDate,
							ActualStartDate, ActualEndDate
                    from Features 
                    where Id = @Id
                    ;
                    select Id, Name, Email, ObjectId from users 
                    where id in (select userid from UserToFeatureAssignments 
                    where featureid = @Id)
                    ;
                    select u.Name, u.Email, u.ObjectId, u.Id from Members m, Users u
                    where m.UserId = u.Id
                    and m.OrganizationId = @OrgId
                    ;
                    select s.Id , s.Name, s.StartDate, s.EndDate
                    from Features f left outer join Sprint s
					on f.SprintId = s.Id
                    where f.Id = @Id;
                    
                    select r.id, r.name, r.releaseDate
					from features f left outer join Releases r
					on r.id = f.ReleaseId
					where f.id = @Id";

                using (IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    var result = await con.QueryMultipleAsync(sql, new
                    {
                        query.Id,
                        query.OrgId
                    });

                    var featureInformation = await result.ReadAsync<GetFeatureDetailsDto>();
                    var assignees = await result.ReadAsync<AssigneeDto>();
                    var members = await result.ReadAsync<OrganizationMemberDto>();
                    var sprint = await result.ReadAsync<SprintDetailsDto>();
                    var release = await result.ReadAsync<ReleaseDto>();
                                        
                    featureInformation.SingleOrDefault().Assignees = assignees.ToList();
                    featureInformation.SingleOrDefault().Members = members.ToList();
                    featureInformation.SingleOrDefault().Sprint = sprint.SingleOrDefault();
                    featureInformation.SingleOrDefault().Release = release.SingleOrDefault();

                    featureDetails = featureInformation.SingleOrDefault();
                }             

                return featureDetails;
            }
        }
    }
}
