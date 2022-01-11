using Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductFocus.Domain.Model
{
    [Table("RolePermissions")]
    public class RolePermission : Entity<long>
    {
        public virtual Role Role { get; set; }
        public virtual Permission Permission { get; set; }
        protected RolePermission()
        {

        }
    }
}
