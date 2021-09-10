using Microsoft.Data.SqlClient;
using ProductFocus.ConnectionString;
using ProductFocus.Domain;
using ProductFocus.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ProductFocus.AppServices
{
    public sealed class GetDomainEventLogQuery : IQuery<GetDomainEventLogDto>
    {
        internal sealed class GetDomainEventLogQueryHandler : IQueryHandler<GetDomainEventLogQuery, GetDomainEventLogDto>
        {
            private readonly QueriesConnectionString _queriesConnectionString;
            public GetDomainEventLogQueryHandler()
            public Task<GetFeatureDetailsDto> Handle(GetDomainEventLogQuery query)
            {
                GetDomainEventLogDto eventLog = new GetDomainEventLogDto();

                string sql = @"
                    select Id, EventTypeName, DomainEventJson, ModuleId,
                        CreatedOn from WorkItemDomainEventLogs";
                using (IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {

                }
            }
        }
    }
}
