﻿using ProductFocus.Domain.Model;
using System;
using System.Collections.Generic;

namespace ProductFocus.Dtos
{
    public sealed class GetKanbanViewListDto
    {
        public string GroupType { get; set; }
        public IList<GetKanbanViewDto> KanbanList { get; set; }
    }
    public sealed class GetKanbanViewDto
    {
        public IList<GroupItem> GroupList { get; set; }
        public IList<FeatureDetail> FeatureDetails { get; set; }
    }

    public sealed class GroupItem
    {
        public long? GroupId { get; set; }
        public string GroupName { get; set; }
        public GroupItem(long? groupId, string groupName)
        {
            GroupId = groupId;
            GroupName = groupName;
        }
        public static bool operator <(GroupItem group1, GroupItem group2)
        {
            return group1.GroupId < group2.GroupId;
        }
        public static bool operator >(GroupItem group1, GroupItem group2)
        {
            return !(group1 < group2);
        }
    }

    public sealed class GetKanbanViewTempDto
    {
        public long Id { get; set; }
        public string GroupName { get; set; }
        public IList<FeatureDetail> FeatureDetails { get; set; }
    }

    public sealed class FeatureDetail
    {
        public long Id { get; set; }
        public long? ModuleId { get; set; }
        public string Title { get; set; }
        public long SprintId { get; set; }
        public string Name { get; set; }
        public int? StoryPoint { get; set; }
        public int WorkCompletionPercentage { get; set; }
        public Status Status { get; set; }
        public bool IsBlocked { get; set; }
        public long OrderNumber { get; set; }
        public WorkItemType WorkItemType { get; set; }
        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public string Remarks { get; set; }
        public bool FunctionalTestability { get; set; }
        public IList<AssigneeDetail> Assignees { get; set; }
        public IList<ScrumDayDto> ScrumDays { get; set; }
    }

    public sealed class AssigneeDetail
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ObjectId { get; set; }
    }

    public sealed class ScrumDayDto
    {
        public long FeatureId { get; set; }
        public DateTime Date { get; set; }
        public int? WorkCompletionPercentage { get; set; }
        public string Comment { get; set; }
    }

    public enum GroupCategoryEnum
    {
        Module = 1,
        Users = 2
    }
}
