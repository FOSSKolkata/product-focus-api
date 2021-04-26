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
        private readonly IList<Module> _modules = new List<Module>();
        public virtual IReadOnlyList<Module> Modules => _modules.ToList();
        protected Product()
        {

        }
        public Product(Organization organization, string name) : this()
        {            
            Organization = organization;
            Name = name;
        }

        public virtual void AddModule(string name)
        {
            var fetchExistingModuleWithSameName = Modules.FirstOrDefault(x => x.Name == name);
            
            if (fetchExistingModuleWithSameName != null)
                throw new Exception($"Module '{name}' already present for this product");
            
            var module = new Module(this, name);
            _modules.Add(module);
        }
    }
}
