using Dapper;
using Microsoft.Data.SqlClient;
using ProductFocus.ConnectionString;
using ProductFocus.Domain;
using ProductFocusApi.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ProductFocus.AppServices
{
    public sealed class GetDomainEventLogQuery : IQuery<List<GetDomainEventLogDto>>
    {
        public long ProductId { get; set; }
        public IList<long> ModuleIds { get; set; }
        public IList<long> UserIds { get; set; }
        public long Offset { get; set; }
        public long Count { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public GetDomainEventLogQuery(long productId, IList<long> moduleIds, IList<long> userIds, long offset, long count, DateTime startDate, DateTime endDate)
        {
            ProductId = productId;
            ModuleIds = moduleIds;
            UserIds = userIds;
            Offset = offset;
            Count = count;
            StartDate = startDate;
            EndDate = endDate;
        }
        internal sealed class GetDomainEventLogQueryHandler : IQueryHandler<GetDomainEventLogQuery, List<GetDomainEventLogDto>>
        {
            private readonly QueriesConnectionString _queriesConnectionString;
            public GetDomainEventLogQueryHandler(QueriesConnectionString queriesConnectionString)
            {
                _queriesConnectionString = queriesConnectionString;
            }
            public async Task<List<GetDomainEventLogDto>> Handle(GetDomainEventLogQuery query)
            {
                List<GetDomainEventLogDto> eventLogList = new List<GetDomainEventLogDto>();

                var builder = new SqlBuilder();
                var sqlBuilder = builder.AddTemplate("SELECT Id, EventTypeName, DomainEventJson, ModuleId, ProductId, CreatedOn, CreatedBy, ModuleName from Workitemdomaineventlogs /**where**/ /**orderby**/");
                builder.Where("ProductId = @ProductId");
                if (query.ModuleIds != null && query.ModuleIds.Count() > 0)
                    builder.Where("ModuleId in @ModuleIds");
                if (query.UserIds != null && query.UserIds.Count() > 0)
                    builder.Where("CreatedById in @UserIds");
                if (query.StartDate.Year != 1 && query.EndDate.Year != 1)
                    builder.Where("CreatedOn BETWEEN @StartDate AND @EndDate");
                builder.OrderBy("CreatedOn DESC offset @Offset rows fetch next @Count rows only");

                string sql = sqlBuilder.RawSql + @";
                        SELECT wi.Id as EventId, u.Id, u.Name, u.Email FROM Features f 
                        INNER JOIN UserToFeatureAssignments uf ON f.Id=uf.FeatureId
                        INNER JOIN WorkItemDomainEventLogs wi ON wi.FeatureId=f.Id
                        INNER JOIN Users u ON uf.UserId=u.Id
                        INNER JOIN Modules m ON m.Id=f.ModuleId
                        INNER JOIN Products p ON m.ProductId=p.Id
                        WHERE p.id = @ProductId";

                using (IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    var result = await con.QueryMultipleAsync(sql, new
                    {
                        ProductId = query.ProductId,
                        ModuleIds = query.ModuleIds,
                        UserIds = query.UserIds,
                        Offset = query.Offset,
                        Count = query.Count,
                        StartDate = query.StartDate,
                        EndDate = query.EndDate.AddDays(1)
                    });

                    eventLogList = (await result.ReadAsync<GetDomainEventLogDto>()).ToList();
                    var userList = (await result.ReadAsync<Owner>()).ToList();
                    foreach(var eventLog in eventLogList)
                    {
                        eventLog.Owners = userList.Where(a => a.EventId == eventLog.Id).ToList();
                    }
                }
                return eventLogList;
            }

        }
    }
}
