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
    public sealed class GetFeatureDetailsQuery : IQuery<GetFeatureDetailsDto>
    {
        public long OrgId { get; set; }
        public long Id { get; set; }
        
        public GetFeatureDetailsQuery(long orgId, long id)
        {
            Id = id;
            OrgId = orgId;
        }

        internal sealed class GetFeatureDetailsQueryHandler : IQueryHandler<GetFeatureDetailsQuery, GetFeatureDetailsDto>
        {
            private readonly QueriesConnectionString _queriesConnectionString;
            private readonly IEmailService _emailService;

            public GetFeatureDetailsQueryHandler(QueriesConnectionString queriesConnectionString, IEmailService emailService)
            {
                _queriesConnectionString = queriesConnectionString;
                _emailService = emailService;
            }
            public async Task<GetFeatureDetailsDto> Handle(GetFeatureDetailsQuery query)
            {
                GetFeatureDetailsDto featureDetails = new GetFeatureDetailsDto();
                
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
                    select u.Name, u.Email, u.ObjectId from Members m, Users u
                    where m.UserId = u.Id
                    and m.OrganizationId = @OrgId
                    ;
                    select s.Id , s.Name, s.StartDate, s.EndDate
                    from Features f left outer join Sprint s
					on f.SprintId = s.Id
                    where f.Id = @Id";

                using (IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    var result = await con.QueryMultipleAsync(sql, new
                    {
                        Id = query.Id,
                        OrgId = query.OrgId
                    });

                    var featureInformation = await result.ReadAsync<GetFeatureDetailsDto>();
                    var assignees = await result.ReadAsync<Assignee>();
                    var members = await result.ReadAsync<OrganizationMember>();
                    var sprint = await result.ReadAsync<SprintDetails>();
                                        
                    featureInformation.SingleOrDefault().Assignees = assignees.ToList();
                    featureInformation.SingleOrDefault().Members = members.ToList();
                    featureInformation.SingleOrDefault().Sprint = sprint.SingleOrDefault();

                    featureDetails = featureInformation.SingleOrDefault();
                }
                
                _emailService.send();                

                return featureDetails;
            }
        }
    }
}
