using ProductFocus.Domain.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductFocus.Domain.Model
{
    [Table("Members")]
    public class Member : Entity<long>
    {
        public virtual Organization Organization { get; private set; }
        public virtual User User { get; private set; }
        public virtual List<Role> Roles { get; private set; }
        public virtual bool IsOwner { get; private set; }
        protected Member()
        {

        }
        private Member(Organization organization, User user, bool isOwner)
        {
            Organization = organization;
            User = user;
            IsOwner = isOwner;
        }

        public static Member CreateInstance(Organization organization, User user, bool isOwner)
        {
            var member = new Member(organization, user, isOwner);
            return member;
        }
    }
}
