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
        public virtual string Description { get; set; }
        public virtual string Status { get; set; }
        public virtual string AssignedTo { get; set; }
        public virtual long FeatureId { get; set; }
        public virtual Feature Feature { get; set; }
    }
}
