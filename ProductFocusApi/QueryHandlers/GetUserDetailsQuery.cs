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
    public sealed class GetUserDetailsQuery : IQuery<List<GetUserDto>>
    {
        public string Email { get; }
        public GetUserDetailsQuery(string email)
        {
            Email = email;
        }

        internal sealed class GetUserDetailsQueryHandler : IQueryHandler<GetUserDetailsQuery, List<GetUserDto>>
        {
            private readonly QueriesConnectionString _queriesConnectionString;
            private readonly IEmailService _emailService;

            public GetUserDetailsQueryHandler(QueriesConnectionString queriesConnectionString, IEmailService emailService)
            {
                _queriesConnectionString = queriesConnectionString;
                _emailService = emailService;
            }
            public async Task<List<GetUserDto>> Handle(GetUserDetailsQuery query)
            {
                List<GetUserDto> userDetails = new List<GetUserDto>();
                
                string sql = @"
                    select id, name, email 
                    from users
                    where email = @Email";

                using (IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    userDetails = (await con.QueryAsync<GetUserDto>(sql, new
                    {
                        Email = query.Email
                    })).ToList();
                }
                
                //_emailService.send();
                
                return userDetails;
            }
        }
    }
}
