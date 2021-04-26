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
        private readonly IList<Feature> _features = new List<Feature>();
        public virtual IReadOnlyList<Feature> Features => _features.ToList();
        protected Module()
        {

        }

        public Module(Product product, string name)
        {
            Product = product;
            Name = name;
        }

        public virtual void AddFeature(string title, string description, int progress)
        {
            var fetchExistingFeatureWithSameName = Features.FirstOrDefault(x => x.Title == title);
            if (fetchExistingFeatureWithSameName != null)
                throw new Exception($"Feature '{title}' already exists in this module");
            var feature = new Feature(this, title, description, progress);
            _features.Add(feature);
        }
    }
}
