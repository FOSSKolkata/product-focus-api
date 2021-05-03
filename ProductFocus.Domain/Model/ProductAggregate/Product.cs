using Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductFocus.Domain.Model
{
    public class Product : AggregateRoot<long>
    {
        public virtual string Name { get; private set; }
        public virtual Organization Organization { get; private set; }
        private readonly IList<Module> _modules = new List<Module>();
        public virtual IReadOnlyList<Module> Modules => _modules.ToList();
        protected Product()
        {

        }
        private Product(Organization organization, string name)
        {            
            Organization = organization;
            Name = name;
        }

        public static Product CreateInstance(Organization organization, string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new Exception("Product name can't be null or empty");

            var product = new Product(organization, name);
            return product;
        }

        public virtual void AddModule(string name)
        {
            var existingModuleWithSameName = Modules.FirstOrDefault(x => x.Name == name);
            
            if (existingModuleWithSameName != null)
                throw new Exception($"Module '{name}' already present for this product");
            
            var module = Module.CreateInstance(this, name);
            _modules.Add(module);
        }
    }
}
