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
    public sealed class GetSprintDetailsQuery : IRequest<List<GetSprintDto>>
    {
        public long ProductId { get; }
        public GetSprintDetailsQuery(long productId)
        {
            ProductId = productId;
        }

        internal sealed class GetSprintDetailsQueryHandler : IRequestHandler<GetSprintDetailsQuery, List<GetSprintDto>>
        {
            private readonly QueriesConnectionString _queriesConnectionString;            

            public GetSprintDetailsQueryHandler(QueriesConnectionString queriesConnectionString)
            {
                _queriesConnectionString = queriesConnectionString;                
            }
            public async Task<List<GetSprintDto>> Handle(GetSprintDetailsQuery query, CancellationToken cancellationToken)
            {
                List<GetSprintDto> sprintList = new();
                
                string sql = @"
                    select Id, Name, StartDate, EndDate 
                    from Sprint
                    where ProductId = @PrdId
                    order by StartDate desc";
                
                using (IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    sprintList = (await con.QueryAsync<GetSprintDto>(sql, new
                    {
                        PrdId = query.ProductId
                    })).ToList();
                }
                
                return sprintList;
            }
        }
    }
}
