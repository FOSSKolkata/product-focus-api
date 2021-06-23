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
            private readonly IEmailService _emailService;

            public GetKanbanViewQueryHandler(QueriesConnectionString queriesConnectionString, IEmailService emailService)
            {
                _queriesConnectionString = queriesConnectionString;
                _emailService = emailService;
            }
            public async Task<List<GetKanbanViewDto>> Handle(GetKanbanViewQuery query)
            {
                List<GetKanbanViewDto> kanbanViewList = new List<GetKanbanViewDto>();

                string sql1 = @"
                    select Id 
                    from Users
                    where ObjectId = @ObjectId";

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

                    
                    var kanbanViews = await result.ReadAsync<GetKanbanViewDto>();
                    var featureDetails = await result.ReadAsync<FeatureDetail>();
                    var assigneeDetails = await result.ReadAsync<AssigneeDetail>();

                    foreach (var kanbanView in kanbanViews)
                    {
                        kanbanView.FeatureDetails = featureDetails.Where(a => a.ModuleId == kanbanView.Id).ToList();

                        foreach (var featureDetail in kanbanView.FeatureDetails)
                        {
                            featureDetail.Assignees = assigneeDetails.Where(a => a.Id == featureDetail.Id).ToList();
                        }
                    }                                        

                    kanbanViewList = kanbanViews.ToList();
                }

                _emailService.send();
                
                return kanbanViewList;
            }
        }
    }
}
