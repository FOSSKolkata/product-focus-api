using ProductFocus.Domain;
using ProductFocus.Dtos;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using ProductFocus.ConnectionString;
using System.Threading.Tasks;

namespace ProductFocus.AppServices
{
    public sealed class GetKanbanViewQuery : IQuery<List<GetKanbanViewDto>>
    {
        public string ObjectId { get; }
        public long Id { get; }

        public GetKanbanViewQuery(long id, string objectId)
        {
            Id = id;
            ObjectId = objectId;
        }

        internal sealed class GetKanbanViewQueryHandler : IQueryHandler<GetKanbanViewQuery, List<GetKanbanViewDto>>
        {
            private readonly QueriesConnectionString _queriesConnectionString;

            public GetKanbanViewQueryHandler(QueriesConnectionString queriesConnectionString)
            {
                _queriesConnectionString = queriesConnectionString;
            }
            public async Task<List<GetKanbanViewDto>> Handle(GetKanbanViewQuery query)
            {
                List<GetKanbanViewDto> kanbanViewList = new List<GetKanbanViewDto>();
                List<GetKanbanViewTempDto> kanbanViewTempList = new List<GetKanbanViewTempDto>();

                string sql1 = @"
                    select Id 
                    from Users
                    where ObjectId = @ObjectId";

                var builder = new SqlBuilder();

                var selector = builder.AddTemplate("SELECT f.Id, u.Id as UserId, u.Name, u.Email, u.ObjectId FROM Features f /**innerjoin**/ /**where**/");
                builder.InnerJoin("UserToFeatureAssignments uf ON f.Id=uf.FeatureId");
                builder.InnerJoin("Users u ON uf.UserId=u.Id");
                builder.InnerJoin("Modules m ON m.Id=f.ModuleId");
                builder.InnerJoin("Products p ON m.ProductId=p.Id");
                builder.Where("p.id = @Prdid");

                var tempStr = selector.RawSql;

                string sql2 = @"
                    SELECT mo.Id, mo.Name 
                    from Organizations o, Members m, Products p, Modules mo
                    WHERE o.Id = m.OrganizationId
                    and o.Id = p.OrganizationId
                    and p.Id = mo.ProductId
                    and mo.productid = @PrdId
                    and m.UserId = @UserId
                    ;
                    SELECT f.Id, ModuleId, Title, SprintId, Name, StoryPoint, WorkCompletionPercentage, Status, IsBlocked, WorkItemType, 
                            PlannedStartDate, PlannedEndDate, 
                            ActualStartDate, ActualEndDate
                    FROM Features f left outer join Sprint s
                    ON f.SprintId = s.Id
					WHERE moduleid in (
                        select id from Modules where productid = @PrdId)
                    ;
                    SELECT f.Id, u.Id as UserId, u.Name, u.Email, u.ObjectId
                    FROM Features f, UserToFeatureAssignments uf, Users u
					Where uf.UserId = u.Id
                    and f.Id = uf.FeatureId									
					and moduleid in (
                        select id from Modules where productid = @PrdId)";

                using (IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    var userId = (await con.QueryAsync<long>(sql1, new
                    {
                        ObjectId = query.ObjectId
                    }));

                    var result = await con.QueryMultipleAsync(sql2, new
                    {
                        PrdId = query.Id,
                        UserId = userId
                    });

                    
                    var kanbanViewsTemp = await result.ReadAsync<GetKanbanViewTempDto>();
                    var featureDetails = await result.ReadAsync<FeatureDetail>();
                    var assigneeDetails = await result.ReadAsync<AssigneeDetail>();

                    foreach (var kanbanView in kanbanViewsTemp)
                    {
                        kanbanView.FeatureDetails = featureDetails.Where(a => a.ModuleId == kanbanView.Id).ToList();

                        foreach (var featureDetail in kanbanView.FeatureDetails)
                        {
                            featureDetail.Assignees = assigneeDetails.Where(a => a.Id == featureDetail.Id).ToList();
                        }
                    }                                        

                    kanbanViewTempList = kanbanViewsTemp.ToList();
                }

                for(int i = 0; i < kanbanViewTempList.Count; i++)
                {
                    kanbanViewList.Add(new GetKanbanViewDto());
                    kanbanViewList[i].GroupList[0].GroupName = kanbanViewTempList[i].GroupName;
                    kanbanViewList[i].FeatureDetails = kanbanViewTempList[i].FeatureDetails;
                }
                
                return kanbanViewList;
            }
        }
    }
}
