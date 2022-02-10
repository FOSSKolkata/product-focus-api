using ProductFocus.Domain.Common;

namespace ProductFocus.Domain.Model
{
    public class Permission : AggregateRoot<long>
    {
        public virtual string Description { get; set; }
        protected Permission()
        {

        }
    }
}
