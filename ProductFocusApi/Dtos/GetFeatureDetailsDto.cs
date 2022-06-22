using ProductFocus.Domain.Model;
using System;
using System.Collections.Generic;

namespace ProductFocus.Dtos
{
    public sealed class GetFeatureDetailsDto
    {
        public long Id { get; set; }
        public string Title { get; set; }        
        public string Description { get; set; }
        public int WorkCompletionPercentage { get; set; }
        public Status Status { get; set; }
        public int StoryPoint { get; set; }
        public bool IsBlocked { get; set; }
        public string AcceptanceCriteria { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public DateTime PlannedEndDate { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime ActualEndDate { get; set; }
        public IList<OrganizationMemberDto> Members { get; set; }
        public IList<AssigneeDto> Assignees { get; set; }
        public SprintDetailsDto Sprint { get; set; }
        public ReleaseDto Release { get; set; }
    }

    public sealed class AssigneeDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ObjectId { get; set; }
    }

    public sealed class OrganizationMemberDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string ObjectId { get; set; }
        public long Id { get; set; }
    }

    public sealed class SprintDetailsDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public sealed class ReleaseDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
    }

}
