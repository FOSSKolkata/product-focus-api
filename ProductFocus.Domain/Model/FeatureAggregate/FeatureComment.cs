using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProductFocus.Domain.Model
{
    [Table("FeatureComments")]
    public class FeatureComment : Entity<long>
    {
        public string Comment { get; set; }
        public long FeatureId { get; set; }
        public Feature Feature { get; set; }
    }
}
