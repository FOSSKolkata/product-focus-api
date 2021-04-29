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
        public virtual Organization Organization { get; private set; }
        public virtual User User { get; private set; }
        public virtual List<Role> Roles { get; private set; }
        public virtual bool IsOwner { get; private set; }
        private Member()
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
