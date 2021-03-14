using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductFocus.Domain.Model
{
    public class Role : Entity<long>
    {
        public string Name { get; set; }
    }
}
