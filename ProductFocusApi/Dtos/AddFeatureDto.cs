using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Dtos
{
    public sealed class AddFeatureDto
    {
        public string Title { get; set; }        
        public string WorkItemType { get; set; }
    }
}
