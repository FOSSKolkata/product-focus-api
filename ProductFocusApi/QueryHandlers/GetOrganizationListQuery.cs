using ProductFocus.Domain.Common;
using ProductFocus.Dtos;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using ProductFocus.ConnectionString;
using System.Threading.Tasks;

namespace ProductFocus.AppServices
{
    public sealed class GetOrganizationListQuery : IQuery<List<GetOrganizationDto>>
    {
        public GetOrganizationListQuery()
        {

        }

        internal sealed class GetOrganizationListQueryHandler : IQueryHandler<GetOrganizationListQuery, List<GetOrganizationDto>>
        {
            private readonly QueriesConnectionString _queriesConnectionString;

            public GetOrganizationListQueryHandler(QueriesConnectionString queriesConnectionString)
            {
                _queriesConnectionString = queriesConnectionString;
            }
            public async Task<List<GetOrganizationDto>> Handle(GetOrganizationListQuery query)
            {
                List<GetOrganizationDto> organizationsList = new();

                using(IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    organizationsList = (await con.QueryAsync<GetOrganizationDto>("select id, name from [product-focus].[dbo].[Organizations]")).ToList();
                }
                
                //_emailService.send();
                
                return organizationsList;
            }
        }
    }
}
