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
    public sealed class GetFeatureDetailsQuery : IQuery<List<GetFeatureDetailsDto>>
    {
        public long Id { get; set; }
        
        public GetFeatureDetailsQuery(long id)
        {
            Id = id;
        }

        internal sealed class GetFeatureDetailsQueryHandler : IQueryHandler<GetFeatureDetailsQuery, List<GetFeatureDetailsDto>>
        {
            private readonly QueriesConnectionString _queriesConnectionString;
            private readonly IEmailService _emailService;

            public GetFeatureDetailsQueryHandler(QueriesConnectionString queriesConnectionString, IEmailService emailService)
            {
                _queriesConnectionString = queriesConnectionString;
                _emailService = emailService;
            }
            public async Task<List<GetFeatureDetailsDto>> Handle(GetFeatureDetailsQuery query)
            {
                List<GetFeatureDetailsDto> featureDetails = new List<GetFeatureDetailsDto>();
                
                string sql = @"
                    select Id, Title, Description, WorkCompletionPercentage, Status, StoryPoint, IsBlocked 
                    from Features 
                    where Id = @Id";

                using (IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    featureDetails = (await con.QueryAsync<GetFeatureDetailsDto>(sql, new
                    {
                        Id = query.Id
                    })).ToList();
                }
                
                _emailService.send();
                
                return featureDetails;
            }
        }
    }
}
