using ProductFocus.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public UpdateColumnIdentifier FieldName { get; set; }
    }

    public enum UpdateColumnIdentifier
    {
        Title = 1,
        Description = 2,
        WorkCompletionPercentage = 3,
        Status = 4,
        Sprint = 5,
        StoryPoint = 6,
        Assignees = 7,
        IsBlocked = 8
    }
}
