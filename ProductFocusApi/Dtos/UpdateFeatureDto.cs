using ProductFocus.Domain.Model;
using System;
using System.Collections.Generic;

namespace ProductFocus.Dtos
{
    public sealed class UpdateFeatureDto
    {
        public long Id { get; set; }
        public string Title { get; set; }        
        public string Description { get; set; }
        public int WorkCompletionPercentage { get; set; }
        public Status Status { get; set; }
        public string SprintName { get; set; }
        public int StoryPoint { get; set; }
        public bool IsBlocked { get; set; }
        public string EmailOfAssignee { get; set; }
        public string AcceptanceCriteria { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public DateTime PlannedEndDate { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime ActualEndDate { get; set; }
        public string Remarks { get; set; }
        public bool FunctionalTestability { get; set; }
        public UpdateColumnIdentifier FieldName { get; set; }
        public string UserId { get; set; }
        public long? ModuleId { get; set; }
        public List<long> IncludeOwnerList { get; set; }
        public List<long> ExcludeOwnerList { get; set; }
        public long? ReleaseId { get; set; }
    }

    public enum UpdateColumnIdentifier
    {
        Title = 1,
        Description = 2,
        WorkCompletionPercentage = 3,
        Status = 4,
        Sprint = 5,
        StoryPoint = 6,        
        IsBlocked = 7,
        IncludeAssignee = 8,
        ExcludeAssignee = 9,
        AcceptanceCriteria = 10,
        PlannedStartDate = 11,
        PlannedEndDate = 12,
        ActualStartDate = 13,
        ActualEndDate = 14,
        Remarks = 15,
        FunctionalTestability = 16,
        UpdateModule = 17,
        IncludeAndExcludeOwners = 18,
        Release = 19
    }
}
