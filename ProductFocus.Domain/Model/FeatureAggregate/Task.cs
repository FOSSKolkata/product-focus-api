using ProductFocus.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

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

        protected Task()
        {

        }
    }
}
