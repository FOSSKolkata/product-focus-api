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
    public sealed class GetOrganizationListQuery : IQuery<List<GetOrganizationDto>>
    {
        public GetOrganizationListQuery()
        {

        }

        internal sealed class GetOrganizationListQueryHandler : IQueryHandler<GetOrganizationListQuery, List<GetOrganizationDto>>
        {
            private readonly QueriesConnectionString _queriesConnectionString;
            private readonly IEmailService _emailService;

            public GetOrganizationListQueryHandler(QueriesConnectionString queriesConnectionString, IEmailService emailService)
            {
                _queriesConnectionString = queriesConnectionString;
                _emailService = emailService;
            }
            public async Task<List<GetOrganizationDto>> Handle(GetOrganizationListQuery query)
            {
                List<GetOrganizationDto> organizationsList = new List<GetOrganizationDto>();

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
