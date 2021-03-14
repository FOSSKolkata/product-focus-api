using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductFocus.Domain.Model
{
    public class RolePermission : Entity<long>
    {
        public Role Role { get; set; }
        public Permission Permission { get; set; }
    }
}
