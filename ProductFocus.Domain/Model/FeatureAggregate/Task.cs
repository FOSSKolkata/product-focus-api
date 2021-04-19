using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Model
{
    [Table("Tasks")]
    public class Task : Entity<long>
    {
        public string Description { get; set; }
        public string Status { get; set; }
        public string AssignedTo { get; set; }
        public long FeatureId { get; set; }
        public Feature Feature { get; set; }
    }
}
