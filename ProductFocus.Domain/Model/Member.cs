using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductFocus.Domain.Model
{
    public class Member : Entity<long>
    {
        public Organization Organization { get; set; }
        public User User { get; set; }
        public List<Role> Roles { get; set; }
        public bool IsOwner { get; set; }
    }
}
