using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductFocus.Domain.Model
{
    public class Role : AggregateRoot<long>
    {
        public string Name { get; set; }
        public List<RolePermission> RolePermissions { get; set; }
        protected Role()
        {

        }
    }
}
