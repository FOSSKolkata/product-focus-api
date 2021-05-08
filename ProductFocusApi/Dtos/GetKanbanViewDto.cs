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
    }
}
