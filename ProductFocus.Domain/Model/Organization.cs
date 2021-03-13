using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Model
{
    public class Organization : Entity<long>
    {
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
