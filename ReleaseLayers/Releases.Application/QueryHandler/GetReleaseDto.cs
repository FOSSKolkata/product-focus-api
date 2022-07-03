using Releases.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Releases.Application.QueryHandler
{
    public class GetReleaseDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ReleaseDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public long FeatureCount { get; set; }
        public long BugCount { get; set; }
        public long EpicCount { get; set; }
        public long PbiCount { get; set; }
        public ReleaseStatus Status { get; set; }
    }
    public class GetReleaseWorkItemCountDto
    {
        public long Id { get; set; }
        public long ReleaseId { get; set; }
        public WorkItemType WorkItemType { get; set; }
        public long WorkItemCount { get; set; }
    }
}
