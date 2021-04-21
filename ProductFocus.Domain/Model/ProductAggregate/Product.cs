using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Model
{
    public class Product : AggregateRoot<long>
    {
        public virtual string Name { get; set; }
        public virtual Organization Organization { get; set; }
        private IList<Module> _modules = new List<Module>();
        public virtual IReadOnlyList<Module> Modules => _modules.ToList();
        protected Product()
        {

        }
        public Product(Organization organization, string name)
        {
            Name = name;
            Organization = organization;
        }

        public virtual void AddModule(string name)
        {
            var module = new Module(this, name);
            _modules.Add(module);
        }
    }
}
