using BusinessRequirements.Domain.Common;

namespace BusinessRequirements.Domain.Model
{
    public class Product : AggregateRoot<long>
    {
        public virtual string Name { get; private set; }
        public virtual Organization Organization { get; private set; }

        protected Product()
        {

        }

        private Product(string name, Organization organization)
        {
            Name = name;
            Organization = organization;
        }

        public static Product CreateInstance(string name, Organization organization)
        {
            return new(name, organization);
        }
    }
}
