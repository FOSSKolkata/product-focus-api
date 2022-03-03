using ProductFocus.Domain.Common;
using System.Collections.Generic;

namespace ProductFocus.Domain.Model
{
    public class Role : AggregateRoot<long>
    {
        public virtual string Name { get; set; }
        public virtual List<RolePermission> RolePermissions { get; set; }
        protected Role()
        {

        }
    }
}
