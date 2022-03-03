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
    public sealed class GetUserDetailsQuery : IRequest<List<GetUserDto>>
    {
        public string Email { get; }
        public GetUserDetailsQuery(string email)
        {
            Email = email;
        }

        internal sealed class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, List<GetUserDto>>
        {
            private readonly QueriesConnectionString _queriesConnectionString;

            public GetUserDetailsQueryHandler(QueriesConnectionString queriesConnectionString)
            {
                _queriesConnectionString = queriesConnectionString;
            }
            public async Task<List<GetUserDto>> Handle(GetUserDetailsQuery query, CancellationToken cancellationToken)
            {
                List<GetUserDto> userDetails = new();
                
                string sql = @"
                    select id, name, email 
                    from users
                    where email = @Email";

                using (IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    userDetails = (await con.QueryAsync<GetUserDto>(sql, new
                    {
                        query.Email
                    })).ToList();
                }
                
                //_emailService.send();
                
                return userDetails;
            }
        }
    }
}
