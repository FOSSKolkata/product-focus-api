using ProductFocus.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Dtos
{
    public sealed class GetKanbanViewDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public IList<FeatureDetail> FeatureDetails { get; set; }
    }

    public sealed class FeatureDetail
    {
        public long Id { get; set; }
        public long ModuleId { get; set; }
        public string Title { get; set; }
        public Status Status { get; set; }
        public bool IsBlocked { get; set; }
        public WorkItemType WorkItemType { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public DateTime PlannedEndDate { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime ActualEndDate { get; set; }
    }
}
