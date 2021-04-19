using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Model
{
    [Table("Modules")]
    public class Module : Entity<long>
    {
        public string Name { get; set; }
        public long ProductId { get; set; }
        public Product Product { get; set; }
        protected Module()
        {

        }

        public Module(Product product, string name)
        {
            Product = product;
            Name = name;
        }
    }
}
