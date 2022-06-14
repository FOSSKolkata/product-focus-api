using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using ProductFocusApi.Dtos;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProductFocusApi.QueryHandlers
{
    public sealed class GetProductByIdQuery : IRequest<GetProductByIdDto>
    {
        public long Id { get; private set; }
        public GetProductByIdQuery(long id)
        {
            Id = id;
        }
        internal sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, GetProductByIdDto>
        {
            private readonly string _queriesConnectionString;
            public GetProductByIdQueryHandler(IConfiguration configuration)
            {
                _queriesConnectionString = configuration["ConnectionStrings:QueriesConnectionString"] ?? throw new NullReferenceException("Connection String is missing from config file");
            }

            public async Task<GetProductByIdDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
            {
                GetProductByIdDto result;
                string sql = "SELECT id, name FROM [dbo].[Products] WHERE id = @Id";
                using(IDbConnection con = new SqlConnection(_queriesConnectionString))
                {
                    result = (await con.QueryAsync<GetProductByIdDto>(sql, new
                    {
                        request.Id
                    })).SingleOrDefault();
                }
                return result;
            }
        }
    }
}
