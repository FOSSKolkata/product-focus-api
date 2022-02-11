using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using ProductFocus.ConnectionString;
using ProductFocusApi.Dtos;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProductFocusApi.QueryHandlers
{
    public sealed class GetBusinessRequirementDetailsQuery : IRequest<GetBusinessRequirementDetailsDto>
    {
        public long Id { get; set; }
        public GetBusinessRequirementDetailsQuery(long id)
        {
            Id = id;
        }
        internal sealed class GetBusinessRequirementDetialsQueryHandler : IRequestHandler<GetBusinessRequirementDetailsQuery, GetBusinessRequirementDetailsDto>
        {
            private readonly QueriesConnectionString _queriesConnectionString;

            public GetBusinessRequirementDetialsQueryHandler(QueriesConnectionString queriesConnectionString)
            {
                _queriesConnectionString = queriesConnectionString;
            }
            public async Task<GetBusinessRequirementDetailsDto> Handle(GetBusinessRequirementDetailsQuery query, CancellationToken cancellationToken)
            {
                GetBusinessRequirementDetailsDto businessRequirementDetails = new();

                string sql = @"SELECT br.Id, br.title, br.ReceivedOn, br.SourceEnum, br.SourceInformation, Description FROM BusinessRequirements br WHERE Id = @Id";

                string sql1 = @"SELECT t.Name, t.Id, brt.BusinessRequirementId FROM Tags t
                                INNER JOIN BusinessRequirementTags brt
                                ON t.Id = brt.TagId WHERE brt.BusinessRequirementId = @Id;";

                using (IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    businessRequirementDetails = (await con.QueryAsync<GetBusinessRequirementDetailsDto>(sql, new
                    {
                        query.Id
                    })).FirstOrDefault();

                    businessRequirementDetails.Tags = (await con.QueryAsync<BusinessRequirementTagDto>(sql1, new 
                    { 
                        query.Id
                    })).ToList();
                }
                return businessRequirementDetails;
            }
        }
    }
}
