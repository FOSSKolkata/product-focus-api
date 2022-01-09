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
        public long RecordOffset { get; set; }
        public long Count { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string EventType { get; set; }
        public GetDomainEventLogQuery(long productId, IList<long> moduleIds, IList<long> userIds, long recordOffset, long count, DateTime? startDate, DateTime? endDate, string eventType)
        {
            ProductId = productId;
            ModuleIds = moduleIds;
            UserIds = userIds;
            StartDate = startDate;
            EndDate = endDate;
            RecordOffset = recordOffset;
            Count = count;
            EventType = eventType;
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
                List<GetDomainEventLogDto> eventLogList = new();

                var builder = new SqlBuilder();
                var sqlBuilder = builder.AddTemplate("SELECT Id, EventTypeName, DomainEventJson, ModuleId, ProductId, CreatedOn, CreatedBy, ModuleName from Workitemdomaineventlogs /**where**/ /**orderby**/");
                builder.Where("ProductId = @ProductId");
                if (query.EventType != null && query.EventType != string.Empty)
                    builder.Where("EventTypeName = @EventType");
                if (query.ModuleIds != null && query.ModuleIds.Count > 0)
                    builder.Where("ModuleId in @ModuleIds");
                if (query.UserIds != null && query.UserIds.Count > 0)
                    builder.Where("CreatedById in @UserIds");
                if (query.StartDate != null && query.EndDate != null)
                    builder.Where("CreatedOn BETWEEN @StartDate AND @EndDate");
                builder.OrderBy("CreatedOn DESC offset @RecordOffset rows fetch next @Count rows only");

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
                        query.ProductId,
                        query.ModuleIds,
                        query.UserIds,
                        query.RecordOffset,
                        query.Count,
                        query.StartDate,
                        EndDate = query.EndDate?.AddDays(1),
                        query.EventType
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
