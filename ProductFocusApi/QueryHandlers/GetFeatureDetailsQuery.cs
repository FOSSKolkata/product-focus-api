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
        public long Id { get; set; }
        
        public GetFeatureDetailsQuery(long id)
        {
            Id = id;
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
                    where featureid = @Id)";

                using (IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    var result = await con.QueryMultipleAsync(sql, new
                    {
                        Id = query.Id
                    });

                    var featureInformation = await result.ReadAsync<GetFeatureDetailsDto>();
                    var assignees = await result.ReadAsync<Assignee>();
                                        
                    featureInformation.SingleOrDefault().Assignees = assignees.ToList();
                    
                    featureDetails = featureInformation.SingleOrDefault();
                }
                
                _emailService.send();                

                return featureDetails;
            }
        }
    }
}
