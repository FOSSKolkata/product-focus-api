using ProductFocus.Dtos;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using ProductFocus.ConnectionString;
using System.Threading.Tasks;
using MediatR;
using System.Threading;

namespace ProductFocus.AppServices
{
    public sealed class GetModuleListQuery : IRequest<List<GetModuleDto>>
    {
        public long Id { get; }
        public GetModuleListQuery(long id)
        {
            Id = id;
        }

        internal sealed class GetModuleListQueryHandler : IRequestHandler<GetModuleListQuery, List<GetModuleDto>>
        {
            private readonly QueriesConnectionString _queriesConnectionString;

            public GetModuleListQueryHandler(QueriesConnectionString queriesConnectionString)
            {
                _queriesConnectionString = queriesConnectionString;
            }
            public async Task<List<GetModuleDto>> Handle(GetModuleListQuery query, CancellationToken cancellationToken)
            {
                List<GetModuleDto> moduleList = new();
                
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
                
                return moduleList;
            }
        }
    }
}
