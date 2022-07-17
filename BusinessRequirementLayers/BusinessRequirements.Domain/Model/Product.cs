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

        private Product(long id, string name, Organization organization)
        {
            Id = id;
            Name = name;
            Organization = organization;
        }

        public static Product CreateInstance(long id, Organization organization, string name)
        {
            return new(id, name, organization);
        }
    }
}
