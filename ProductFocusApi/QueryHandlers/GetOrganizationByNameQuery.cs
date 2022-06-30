using Dapper;
using MediatR;
using ProductFocus.ConnectionString;
using ProductFocusApi.Dtos;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProductFocusApi.QueryHandlers
{
    public sealed class GetOrganizationByNameQuery : IRequest<GetOrganizationByNameDto>
    {
        public string Name { get; private set; }
        public string UserObjectId { get; private set; }
        public GetOrganizationByNameQuery(string name, string userObjectId)
        {
            Name = name;
            UserObjectId = userObjectId;
        }
        internal class GetOrganizationByNameQueryHander : IRequestHandler<GetOrganizationByNameQuery, GetOrganizationByNameDto>
        {
            private readonly QueriesConnectionString _queriesConnectionString;
            public GetOrganizationByNameQueryHander(QueriesConnectionString queriesConnectionString)
            {
                _queriesConnectionString = queriesConnectionString;
            }

            public async Task<GetOrganizationByNameDto> Handle(GetOrganizationByNameQuery request, CancellationToken cancellationToken)
            {
                GetOrganizationByNameDto organization;
                string sql = @"SELECT id, name FROM [dbo].[Organizations] WHERE name = @Name";
                using(IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    organization = (await con.QueryAsync<GetOrganizationByNameDto>(sql, new
                    {
                        request.Name
                    })).SingleOrDefault();
                }
                return organization;
            }
        }
    }
}
