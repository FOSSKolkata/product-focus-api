using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Model
{
    public class Feature : Entity<long>
    {
        public string Title { get; set; }
        public List<Task> Tasks { get; set; }
        public string Owner { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public DateTime PlannedEndDate { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime ActualEndDate { get; set; }
        public string Comment { get; set; }
        public string CommentedBy { get; set; }
        public DateTime CommentTimestamp { get; set; }
        public int WorkPgressIndicator { get; set; }
        public string Status { get; set; }
        public int ModuleId { get; set; }
        public Module Module { get; set; }
    }
}
