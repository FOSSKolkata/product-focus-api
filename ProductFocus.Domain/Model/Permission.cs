using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductFocus.Domain.Model
{
    public class Permission : Entity<long>
    {
        public string Description { get; set; }
    }
}
