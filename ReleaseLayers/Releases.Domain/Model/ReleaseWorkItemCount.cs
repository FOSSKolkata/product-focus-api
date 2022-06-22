using Releases.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Releases.Domain.Model
{
    public class ReleaseWorkItemCount : Entity<long>
    {
        public WorkItemType WorkItemType { get; private set; }
        public long WorkItemCount { get; private set; }
    }
    public enum WorkItemType
    {
        Feature = 1,
        Bug = 2,
        Epic = 3,
        PBI = 4
    }
}
