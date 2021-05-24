using ProductFocus.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }

}
