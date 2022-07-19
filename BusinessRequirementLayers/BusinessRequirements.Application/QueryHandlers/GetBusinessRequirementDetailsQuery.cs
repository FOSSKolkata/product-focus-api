using BusinessRequirements.QueryHandlers.Dtos;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace BusinessRequirements.QueryHandlers
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
            private readonly string _queriesConnectionString;

            public GetBusinessRequirementDetialsQueryHandler(IConfiguration configuration)
            {
                _queriesConnectionString = configuration["ConnectionStrings:QueriesConnectionString"] ?? throw new NullReferenceException("Connection String is missing from config file");
            }
            public async Task<GetBusinessRequirementDetailsDto> Handle(GetBusinessRequirementDetailsQuery query, CancellationToken cancellationToken)
            {
                GetBusinessRequirementDetailsDto businessRequirementDetails = new();

                string sql = @"SELECT br.Id, br.title, br.ReceivedOn, br.SourceEnum, br.SourceInformation, Description FROM [businessrequirement].[BusinessRequirements] br WHERE Id = @Id";

                string sql1 = @"SELECT t.Name, t.Id, brt.BusinessRequirementId FROM Tags t
                                INNER JOIN BusinessRequirementTags brt
                                ON t.Id = brt.TagId WHERE brt.BusinessRequirementId = @Id;";

                using (IDbConnection con = new SqlConnection(_queriesConnectionString))
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
