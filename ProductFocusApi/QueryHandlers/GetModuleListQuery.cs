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
    public sealed class GetModuleListQuery : IQuery<List<GetModuleDto>>
    {
        public long Id { get; }
        public GetModuleListQuery(long id)
        {
            Id = id;
        }

        internal sealed class GetModuleListQueryHandler : IQueryHandler<GetModuleListQuery, List<GetModuleDto>>
        {
            private readonly QueriesConnectionString _queriesConnectionString;
            private readonly IEmailService _emailService;

            public GetModuleListQueryHandler(QueriesConnectionString queriesConnectionString, IEmailService emailService)
            {
                _queriesConnectionString = queriesConnectionString;
                _emailService = emailService;
            }
            public async Task<List<GetModuleDto>> Handle(GetModuleListQuery query)
            {
                List<GetModuleDto> moduleList = new List<GetModuleDto>();
                
                string sql = @"
                    SELECT id, name 
                    from [product-focus].[dbo].[Modules]
                    WHERE productid = @PrdId";
                
                using (IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    moduleList = (await con.QueryAsync<GetModuleDto>(sql, new
                    {
                        PrdId = query.Id
                    })).ToList();
                }
                
                //_emailService.send();
                
                return moduleList;
            }
        }
    }
}
