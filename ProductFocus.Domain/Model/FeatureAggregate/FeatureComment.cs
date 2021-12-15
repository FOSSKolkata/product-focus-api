using Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductFocus.Domain.Model
{
    [Table("FeatureComments")]
    public class FeatureComment : Entity<long>
    {
        public virtual string Comment { get; set; }
        public virtual long FeatureId { get; set; }
        public virtual Feature Feature { get; set; }

        protected FeatureComment()
        {

        }
    }
}
