using ProductFocus.Domain;
using ProductFocus.Dtos;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using ProductFocus.ConnectionString;
using System.Threading.Tasks;
using System;

namespace ProductFocus.AppServices
{
    public sealed class GetKanbanViewFilterQuery : IQuery<GetKanbanViewListDto>
    {
        public string ObjectId { get; }
        public long Id { get; }
        public long? SprintId { get; set; }
        public IList<long> UserIds { get; set; }
        public GroupCategoryEnum GroupCategoryEnum { get; set; }

        public GetKanbanViewFilterQuery(long id, string objectId, long? sprintId, IList<long> userIds, GroupCategoryEnum groupCategoryEnum)
        {
            Id = id;
            ObjectId = objectId;
            SprintId = sprintId;
            UserIds = userIds;
            GroupCategoryEnum = groupCategoryEnum;
        }

        internal sealed class GetKanbanViewFilterQueryHandler : IQueryHandler<GetKanbanViewFilterQuery, GetKanbanViewListDto>
        {
            private readonly QueriesConnectionString _queriesConnectionString;

            public GetKanbanViewFilterQueryHandler(QueriesConnectionString queriesConnectionString)
            {
                _queriesConnectionString = queriesConnectionString;
            }
            public async Task<GetKanbanViewListDto> Handle(GetKanbanViewFilterQuery query)
            {
                List<GetKanbanViewDto> kanbanViewList = new();
                List<GetKanbanViewDto> userGroupKanban = new();
                List<GetKanbanViewTempDto> kanbanViewTempList = new();
                // Will Store userIds and List of Features with (userIds and names)
                Dictionary<string, List<KeyValuePair<List<GroupItem>,FeatureDetail>>> dic = new();

                string sql1 = @"
                    select Id 
                    from Users
                    where ObjectId = @ObjectId";

                var builder1 = new SqlBuilder();
                var selector1 = builder1.AddTemplate("SELECT distinct f.Id, ModuleId, Title, f.SprintId, s.Name, StoryPoint, WorkCompletionPercentage, Status, IsBlocked, WorkItemType, PlannedStartDate, PlannedEndDate, ActualStartDate, ActualEndDate, Remarks, fo.OrderNumber, FunctionalTestability from Features f /**leftjoin**/ /**innerjoin**/ /**where**/");
                builder1.LeftJoin("FeatureOrderings fo ON fo.FeatureId = f.Id AND fo.SprintId = @SprintId");
                builder1.LeftJoin("Modules m ON (f.ModuleId = m.Id OR f.ModuleId is null)");
                builder1.InnerJoin("Sprint s ON f.SprintId = s.Id");
                builder1.InnerJoin("Products p ON m.ProductId = p.Id OR f.ProductId = p.Id");
                if (query.UserIds != null && query.UserIds.Count > 0)
                    builder1.InnerJoin("UserToFeatureAssignments uf ON f.Id = uf.FeatureId");
                builder1.Where("p.id = @PrdId");
                builder1.Where("s.id = @SprintId");
                if (query.UserIds != null && query.UserIds.Count > 0)
                    builder1.Where("uf.userid in @UserIds");

                var tempStr1 = selector1.RawSql;
                var builder2 = new SqlBuilder();

                var selector2 = builder2.AddTemplate("SELECT f.Id, u.Id as UserId, u.Name, u.Email, u.ObjectId FROM Features f /**leftjoin**/ /**innerjoin**/ /**where**/");
                builder2.LeftJoin("Modules m ON m.Id=f.ModuleId");
                builder2.LeftJoin("Products p ON m.ProductId=p.Id");
                builder2.InnerJoin("UserToFeatureAssignments uf ON f.Id=uf.FeatureId");
                builder2.InnerJoin("Users u ON uf.UserId=u.Id");
                builder2.Where("f.productId = @PrdId");

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
                        query.UserIds
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
                            List<GroupItem> groupItemList = new();  // Will store list of UserId and Name
                            string userIds = string.Empty;          // Will Store ',' separated UserIds

                            foreach (var assignee in featureDetail.Assignees)
                            {
                                groupItemList.Add(new GroupItem(assignee.UserId, assignee.Name));
                                userIds = userIds + assignee.UserId + ",";
                            }
                            if (groupItemList.Count != 0)
                            {
                                if (!dic.ContainsKey(userIds))      // If List of UserIds doesn't exist in dictionary
                                {
                                    // Initialize
                                    dic[userIds] = new List<KeyValuePair<List<GroupItem>,FeatureDetail>>();
                                }
                                // Adding the feature with UserList into the dictionary
                                dic[userIds].Add(new KeyValuePair<List<GroupItem>, FeatureDetail>(groupItemList, featureDetail));
                            }
                            // Fill in scrum days data 
                            featureDetail.ScrumDays = scrumDays.Where(x => x.FeatureId == featureDetail.Id).OrderBy(x => x.Date).ToList();
                        }
                    }
                    kanbanViewTempList = kanbanViewsTemp.ToList();

                    // Feature list which don't have any module assigned
                    List<FeatureDetail> anonymousFeature = featureDetails.Where(a => a.ModuleId == null).ToList();

                    for (int i = 0; i < kanbanViewTempList.Count; i++)
                    {
                        kanbanViewList.Add(new GetKanbanViewDto());
                        kanbanViewList[i].FeatureDetails = kanbanViewTempList[i].FeatureDetails;
                        kanbanViewList[i].GroupList = new List<GroupItem>{new GroupItem(kanbanViewTempList[i].Id, kanbanViewTempList[i].GroupName)};
                    }

                    kanbanViewList.Add(new GetKanbanViewDto());
                    kanbanViewList[^1].FeatureDetails = anonymousFeature;
                    kanbanViewList[^1].GroupList = new List<GroupItem>{new GroupItem(null, "Anonymous Module")};

                    // Same algorithm applied as above in anonymous feature list
                    foreach (var featureDetail in kanbanViewList[^1].FeatureDetails)
                    {
                        featureDetail.Assignees = assigneeDetails.Where(a => a.Id == featureDetail.Id).ToList();
                        List<GroupItem> groupItemList = new();
                        string userIds = string.Empty;
                        foreach (var assignee in featureDetail.Assignees)
                        {
                            groupItemList.Add(new GroupItem(assignee.UserId, assignee.Name));
                            userIds = userIds + assignee.UserId + ",";
                        }
                        if (groupItemList.Count != 0)
                        {
                            if (!dic.ContainsKey(userIds))
                            {
                                dic[userIds] = new List<KeyValuePair<List<GroupItem>, FeatureDetail>>();
                            }
                            dic[userIds].Add(new KeyValuePair<List<GroupItem>, FeatureDetail>(groupItemList, featureDetail));
                        }
                        // Fill in scrum days data 
                        featureDetail.ScrumDays = scrumDays.Where(x => x.FeatureId == featureDetail.Id).OrderBy(x => x.Date).ToList();
                    }

                    // Iterating in dictionary to generate GroupByUser
                    foreach (KeyValuePair<string, List<KeyValuePair<List<GroupItem>, FeatureDetail>>> kv in dic)
                    {
                        GetKanbanViewDto tempKanbanBoard = new();
                        tempKanbanBoard.GroupList = kv.Value[0].Key;    // Taking 0 since every list item has same user
                        List<FeatureDetail> featureDetailsList = new(); // Will store all the feature which is under all the above user.
                        foreach(KeyValuePair<List<GroupItem>, FeatureDetail> groupItemListAndFeatureDetail in kv.Value)
                        {
                            featureDetailsList.Add(groupItemListAndFeatureDetail.Value);
                        }
                        tempKanbanBoard.FeatureDetails = featureDetailsList;
                        userGroupKanban.Add(tempKanbanBoard);
                    }

                    // Feature list without user assigned to
                    List<FeatureDetail> anonymousFeatureWithoutUser = featureDetails.Where(a => a.Assignees.Count == 0 || a.Assignees == null).ToList();

                    userGroupKanban.Add(new GetKanbanViewDto());
                    userGroupKanban[^1].GroupList = new List<GroupItem>
                    {
                        new GroupItem(null, "Anonymous User")
                    };
                    userGroupKanban[^1].FeatureDetails = anonymousFeatureWithoutUser;
                    foreach (var feature in userGroupKanban[^1].FeatureDetails)
                    {
                        feature.Assignees = assigneeDetails.Where(a => a.Id == feature.Id).ToList();
                    }
                }
                if(query.GroupCategoryEnum == GroupCategoryEnum.Module)
                {
                    GetKanbanViewListDto kanban = new();
                    kanban.GroupType = "Module";
                    kanban.KanbanList = kanbanViewList;
                    return kanban;
                }

                if(query.GroupCategoryEnum == GroupCategoryEnum.Users)
                {
                    GetKanbanViewListDto kanban = new();
                    kanban.GroupType = "Users";
                    kanban.KanbanList = userGroupKanban;
                    return kanban;
                }
                throw new ArgumentException("Invalid agrument is passed.");
            }
        }
    }
}
