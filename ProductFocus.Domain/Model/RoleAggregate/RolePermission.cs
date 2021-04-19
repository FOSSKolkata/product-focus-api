using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProductFocus.Domain.Model
{
    [Table("RolePermissions")]
    public class RolePermission : Entity<long>
    {
        public Role Role { get; set; }
        public Permission Permission { get; set; }
        protected RolePermission()
        {

        }
    }
}
