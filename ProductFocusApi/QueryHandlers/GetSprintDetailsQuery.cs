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
                    SELECT Id, Name, StartDate, EndDate 
                    FROM Sprint
                    WHERE ProductId = @PrdId AND isdeleted = 'false'
                    ORDER BY StartDate DESC";
                
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
