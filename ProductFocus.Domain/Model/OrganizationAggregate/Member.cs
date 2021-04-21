using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProductFocus.Domain.Model
{
    [Table("Members")]
    public class Member : Entity<long>
    {
        public virtual Organization Organization { get; set; }
        public virtual User User { get; set; }
        public virtual List<Role> Roles { get; set; }
        public virtual bool IsOwner { get; set; }
        protected Member()
        {

        }
        public Member(Organization organization, User user, bool isOwner)
        {
            Organization = organization;
            User = user;
            IsOwner = isOwner;
        }
    }
}
