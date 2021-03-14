using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductFocus.Domain.Model
{
    public class FeatureComment : Entity<long>
    {
        public string Comment { get; set; }
        public long FeatureId { get; set; }
        public Feature Feature { get; set; }
    }
}
