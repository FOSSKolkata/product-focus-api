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
    public sealed class GetKanbanViewFilterQuery : IQuery<List<GetKanbanViewDto>>
    {
        public string ObjectId { get; }
        public long Id { get; }
        public long SprintId { get; set; }
        public IList<long> UserIds { get; set; }

        public GetKanbanViewFilterQuery(long id, string objectId, long sprintId, IList<long> userIds)
        {
            Id = id;
            ObjectId = objectId;
            SprintId = sprintId;
            UserIds = userIds;
        }

        internal sealed class GetKanbanViewFilterQueryHandler : IQueryHandler<GetKanbanViewFilterQuery, List<GetKanbanViewDto>>
        {
            private readonly QueriesConnectionString _queriesConnectionString;
            private readonly IEmailService _emailService;

            public GetKanbanViewFilterQueryHandler(QueriesConnectionString queriesConnectionString, IEmailService emailService)
            {
                _queriesConnectionString = queriesConnectionString;
                _emailService = emailService;
            }
            public async Task<List<GetKanbanViewDto>> Handle(GetKanbanViewFilterQuery query)
            {
                List<GetKanbanViewDto> kanbanViewList = new List<GetKanbanViewDto>();

                string sql1 = @"
                    select Id 
                    from Users
                    where ObjectId = @ObjectId";

                var builder1 = new SqlBuilder();
                var selector1 = builder1.AddTemplate("SELECT distinct f.Id, ModuleId, Title, SprintId, s.Name, StoryPoint, WorkCompletionPercentage, Status, IsBlocked, WorkItemType, PlannedStartDate, PlannedEndDate, ActualStartDate, ActualEndDate from Features f /**innerjoin**/ /**where**/");
                builder1.InnerJoin("Sprint s ON f.SprintId = s.Id");
                builder1.InnerJoin("Modules m ON f.ModuleId = m.Id");
                builder1.InnerJoin("Products p ON m.ProductId = p.Id");
                if (query.UserIds != null && query.UserIds.Count()>0)
                    builder1.InnerJoin("UserToFeatureAssignments uf ON f.Id = uf.FeatureId");
                builder1.Where("p.id = @PrdId");
                builder1.Where("s.id = @SprintId");
                if (query.UserIds != null && query.UserIds.Count() > 0)
                    builder1.Where("uf.userid in @UserIds");

                var tempStr1 = selector1.RawSql;

                var builder2 = new SqlBuilder();

                var selector2 = builder2.AddTemplate("SELECT f.Id, u.Id as UserId, u.Name, u.Email, u.ObjectId FROM Features f /**innerjoin**/ /**where**/");
                builder2.InnerJoin("UserToFeatureAssignments uf ON f.Id=uf.FeatureId");
                builder2.InnerJoin("Users u ON uf.UserId=u.Id");
                builder2.InnerJoin("Modules m ON m.Id=f.ModuleId");
                builder2.InnerJoin("Products p ON m.ProductId=p.Id");
                builder2.Where("p.id = @PrdId");

                var tempStr2 = selector2.RawSql;

                var builder3 = new SqlBuilder();
                var selector3 = builder3.AddTemplate("SELECT  sd.FeatureId, sd.ScrumDate AS Date, sd.WorkCompletionPercentage, sd.Comment FROM ScrumDay sd  /**innerjoin**/ /**where**/");
                builder3.InnerJoin("Features f ON sd.FeatureId = f.Id");
                builder3.InnerJoin("Sprint s ON f.SprintId = s.Id");
                builder3.Where("s.id = @SprintId");

                var tempStr3 = selector3.RawSql;

                string sql2 = @"
                    SELECT mo.Id, mo.Name 
                    from Organizations o, Members m, Products p, Modules mo
                    WHERE o.Id = m.OrganizationId
                    and o.Id = p.OrganizationId
                    and p.Id = mo.ProductId
                    and mo.productid = @PrdId
                    and m.UserId = @UserId
                    ;" + tempStr1 +
                    ";" + tempStr2 + 
                    ";" + tempStr3;
                

                // Query to get scrum data 
                // Select work completion percentage, scrum date and scrum comment for the features which meet filter criteria 

                using (IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    var userId = (await con.QueryAsync<long>(sql1, new
                    {
                        ObjectId = query.ObjectId
                    }));

                    var result = await con.QueryMultipleAsync(sql2, new
                    {
                        PrdId = query.Id,
                        UserId = userId,
                        SprintId = query.SprintId,
                        UserIds = query.UserIds
                    });

                    
                    var kanbanViews = await result.ReadAsync<GetKanbanViewDto>();
                    var featureDetails = await result.ReadAsync<FeatureDetail>();
                    var assigneeDetails = await result.ReadAsync<AssigneeDetail>();
                    var scrumDays = await result.ReadAsync<ScrumDayDto>();

                    foreach (var kanbanView in kanbanViews)
                    {
                        kanbanView.FeatureDetails = featureDetails.Where(a => a.ModuleId == kanbanView.Id).ToList();

                        foreach (var featureDetail in kanbanView.FeatureDetails)
                        {
                            featureDetail.Assignees = assigneeDetails.Where(a => a.Id == featureDetail.Id).ToList();

                            // Fill in scrum days data 
                            featureDetail.ScrumDays = scrumDays.Where(x => x.FeatureId == featureDetail.Id).OrderBy(x => x.Date).ToList();
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
