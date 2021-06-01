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
    public sealed class GetSprintDetailsQuery : IQuery<List<GetSprintDto>>
    {
        public long Id { get; }
        public GetSprintDetailsQuery(long id)
        {
            Id = id;
        }

        internal sealed class GetSprintDetailsQueryHandler : IQueryHandler<GetSprintDetailsQuery, List<GetSprintDto>>
        {
            private readonly QueriesConnectionString _queriesConnectionString;            

            public GetSprintDetailsQueryHandler(QueriesConnectionString queriesConnectionString)
            {
                _queriesConnectionString = queriesConnectionString;                
            }
            public async Task<List<GetSprintDto>> Handle(GetSprintDetailsQuery query)
            {
                List<GetSprintDto> sprintList = new List<GetSprintDto>();
                
                string sql = @"
                    select Id, Name, StartDate, EndDate 
                    from Sprint
                    where ProductId = @PrdId";
                
                using (IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    sprintList = (await con.QueryAsync<GetSprintDto>(sql, new
                    {
                        PrdId = query.Id
                    })).ToList();
                }
                
                return sprintList;
            }
        }
    }
}
