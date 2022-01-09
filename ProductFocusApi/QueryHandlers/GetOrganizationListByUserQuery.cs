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
    public sealed class GetOrganizationListByUserQuery : IQuery<List<GetOrganizationByUserDto>>
    {
        public string ObjectId { get; }
        public GetOrganizationListByUserQuery(string objectId)
        {
            ObjectId = objectId;
        }

        internal sealed class GetOrganizationListByUserQueryHandler : IQueryHandler<GetOrganizationListByUserQuery, List<GetOrganizationByUserDto>>
        {
            private readonly QueriesConnectionString _queriesConnectionString;

            public GetOrganizationListByUserQueryHandler(QueriesConnectionString queriesConnectionString)
            {
                _queriesConnectionString = queriesConnectionString;
            }
            public async Task<List<GetOrganizationByUserDto>> Handle(GetOrganizationListByUserQuery query)
            {
                List<GetOrganizationByUserDto> organizationList = new();                

                string sql1 = @"
                    select Id 
                    from Users
                    where ObjectId = @ObjectId";

                string sql2 = @"
                    select o.Id, o.Name, m.IsOwner 
                    from Organizations o, Members m
                    where o.Id = m.OrganizationId
                    and UserId = @UserId";
                

                using (IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    var userId = (await con.QueryAsync<long>(sql1, new
                    {
                        query.ObjectId
                    }));

                    organizationList = (await con.QueryAsync<GetOrganizationByUserDto>(sql2, new
                    {
                        UserId = userId
                    })).ToList();                    
                }
                
                //_emailService.send();
                
                return organizationList;
            }
        }
    }
}
