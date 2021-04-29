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
        public virtual string Name { get; private set; }
        public virtual long ProductId { get; private set; }
        public virtual Product Product { get; private set; }
        
        private Module()
        {

        }

        private Module(Product product, string name)
        {
            Product = product;
            Name = name;
        }

        public static Module CreateInstance(Product product, string name)
        {
            var module = new Module(product, name);
            return module;
        }
    }
}
