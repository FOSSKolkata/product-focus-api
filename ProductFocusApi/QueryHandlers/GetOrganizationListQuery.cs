using ProductFocus.Domain;
using ProductFocus.Dtos;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using ProductFocus.ConnectionString;

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
            public List<GetOrganizationDto> Handle(GetOrganizationListQuery query)
            {
                List<GetOrganizationDto> organizationsList = new List<GetOrganizationDto>();

                using(IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    organizationsList = con.Query<GetOrganizationDto>("select name from [product-focus].[dbo].[Organizations]").ToList();
                }
                return organizationsList;
            }
        }
    }
}
