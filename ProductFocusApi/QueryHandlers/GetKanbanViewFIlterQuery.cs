using ProductFocus.Domain;
using ProductFocus.Dtos;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using ProductFocus.ConnectionString;
using System.Threading.Tasks;
using ProductFocus.Domain.Model;
using System;

namespace ProductFocus.AppServices
{
    public sealed class GetKanbanViewFilterQuery : IQuery<List<GetKanbanViewDto>>
    {
        public string ObjectId { get; }
        public long Id { get; }
        public long SprintId { get; set; }
        public IList<long> UserIds { get; set; }
        public long OrderingCategoryNum { get; set; }
        public GroupCategoryEnum GroupCategoryEnum { get; set; }

        public GetKanbanViewFilterQuery(long id, OrderingCategoryEnum orderingCategory, string objectId, long sprintId, IList<long> userIds, GroupCategoryEnum groupCategoryEnum)
        {
            Id = id;
            OrderingCategoryNum = ((long)orderingCategory);
            ObjectId = objectId;
            SprintId = sprintId;
            UserIds = userIds;
            GroupCategoryEnum = groupCategoryEnum;
        }

        internal sealed class GetKanbanViewFilterQueryHandler : IQueryHandler<GetKanbanViewFilterQuery, List<GetKanbanViewDto>>
        {
            private readonly QueriesConnectionString _queriesConnectionString;

            public GetKanbanViewFilterQueryHandler(QueriesConnectionString queriesConnectionString)
            {
                _queriesConnectionString = queriesConnectionString;
            }
            public async Task<List<GetKanbanViewDto>> Handle(GetKanbanViewFilterQuery query)
            {
                List<GetKanbanViewDto> kanbanViewList = new();
                List<GetKanbanViewTempDto> kanbanViewTempList = new();
                Dictionary<KeyValuePair<string,string>, List<FeatureDetail>> dic = new();

                string sql1 = @"
                    select Id 
                    from Users
                    where ObjectId = @ObjectId";

                var builder1 = new SqlBuilder();
                var selector1 = builder1.AddTemplate("SELECT distinct f.Id, ModuleId, Title, f.SprintId, s.Name, StoryPoint, WorkCompletionPercentage, Status, IsBlocked, WorkItemType, PlannedStartDate, PlannedEndDate, ActualStartDate, ActualEndDate, Remarks, fo.OrderNumber, FunctionalTestability from Features f /**innerjoin**/ /**leftjoin**/ /**where**/");
                builder1.InnerJoin("Sprint s ON f.SprintId = s.Id");
                builder1.InnerJoin("Modules m ON f.ModuleId = m.Id");
                builder1.InnerJoin("Products p ON m.ProductId = p.Id");
                if (query.UserIds != null && query.UserIds.Count > 0)
                    builder1.InnerJoin("UserToFeatureAssignments uf ON f.Id = uf.FeatureId");
                builder1.LeftJoin("FeatureOrderings fo ON fo.FeatureId = f.Id AND fo.SprintId = @SprintId AND fo.OrderingCategory = @OrderingCategory");
                builder1.Where("p.id = @PrdId");
                builder1.Where("s.id = @SprintId");
                if (query.UserIds != null && query.UserIds.Count > 0)
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
                    SELECT mo.Id, mo.Name as GroupName
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
                    var userId = await con.QueryAsync<long>(sql1, new
                    {
                        query.ObjectId
                    });

                    var result = await con.QueryMultipleAsync(sql2, new
                    {
                        PrdId = query.Id,
                        UserId = userId,
                        query.SprintId,
                        query.UserIds,
                        OrderingCategory = query.OrderingCategoryNum
                    });

                    
                    var kanbanViewsTemp = await result.ReadAsync<GetKanbanViewTempDto>();
                    var featureDetails = await result.ReadAsync<FeatureDetail>();
                    var assigneeDetails = await result.ReadAsync<AssigneeDetail>();
                    var scrumDays = await result.ReadAsync<ScrumDayDto>();

                    foreach (var kanbanView in kanbanViewsTemp)
                    {
                        kanbanView.FeatureDetails = featureDetails.Where(a => a.ModuleId == kanbanView.Id).ToList();

                        foreach (var featureDetail in kanbanView.FeatureDetails)
                        {
                            featureDetail.Assignees = assigneeDetails.Where(a => a.Id == featureDetail.Id).ToList();
                            List<KeyValuePair<string,string>> emailNamePairs = new();
                            foreach(var assignee in featureDetail.Assignees)
                            {
                                emailNamePairs.Add(KeyValuePair.Create(assignee.Email, assignee.Name));
                            }
                            emailNamePairs.Sort((KeyValuePair<string,string> a,KeyValuePair<string,string> b) => {
                                return string.Compare(a.Key, b.Key);
                            });
                            string tempEmails = string.Empty;
                            string tempNames = string.Empty;
                            foreach(KeyValuePair<string,string> keyValue in emailNamePairs)
                            {
                                tempEmails = tempEmails + keyValue.Key + ",";
                                tempNames = tempNames + keyValue.Value + ", ";
                            }
                            if(tempEmails != string.Empty)
                            {
                                KeyValuePair<string,string> key = KeyValuePair.Create(tempEmails, tempNames);
                                if (!dic.ContainsKey(key))
                                {
                                    dic[key] = new List<FeatureDetail>();
                                }
                                dic[key].Add(featureDetail);
                            }
                            // Fill in scrum days data 
                            featureDetail.ScrumDays = scrumDays.Where(x => x.FeatureId == featureDetail.Id).OrderBy(x => x.Date).ToList();
                        }
                    }
                    kanbanViewTempList = kanbanViewsTemp.ToList();
                    for (int i = 0; i < kanbanViewTempList.Count; i++)
                    {
                        kanbanViewList.Add(new GetKanbanViewDto());
                        kanbanViewList[i].FeatureDetails = kanbanViewTempList[i].FeatureDetails;
                        kanbanViewList[i].GroupName = kanbanViewTempList[i].GroupName;
                    }
                }
                if(query.GroupCategoryEnum == GroupCategoryEnum.Module)
                {
                    return kanbanViewList;
                }

                List<GetKanbanViewDto> userGroupKanban = new();
                foreach(KeyValuePair<KeyValuePair<string,string>,List<FeatureDetail>> kv in dic)
                {
                    GetKanbanViewDto temp = new();
                    temp.GroupName = kv.Key.Value[0..^2];
                    temp.FeatureDetails = kv.Value;
                    userGroupKanban.Add(temp);
                }
                if(query.GroupCategoryEnum == GroupCategoryEnum.Users)
                {
                    return userGroupKanban;
                }
                throw new ArgumentException("Invalid agrument is passed.");
            }
        }
    }
}
