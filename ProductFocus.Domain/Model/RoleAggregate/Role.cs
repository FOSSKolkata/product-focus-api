using Common;
using System;
using System.Collections.Generic;
using System.Text;

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
