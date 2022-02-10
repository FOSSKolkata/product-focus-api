using ProductFocus.Domain.Common;
using CSharpFunctionalExtensions;
using System;

namespace ProductFocus.Domain.Model
{
    public class TagCategory : AggregateRoot<long>
    {
        public virtual string Name { get; private set; }
        public virtual Product Product { get; private set; }

        protected TagCategory()
        {
            // this protected constructor is for lazy loading to work
        }
        private TagCategory(Product product, string name)
        {
            Name = name;
            Product = product;
        }

        public static TagCategory CreateInstance(Product product, string name)
        {
            var tagCategory = new TagCategory(product, name);
            return tagCategory;
        }

        public static implicit operator TagCategory(long v)
        {
            throw new NotImplementedException();
        }
    }
}
