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
        public virtual string Name { get; set; }
        public virtual long ProductId { get; set; }
        public virtual Product Product { get; set; }
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
