using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Model
{
    public class Module : Entity<long>
    {
        public string Name { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public List<Feature> Features { get; set; }
    }
}
