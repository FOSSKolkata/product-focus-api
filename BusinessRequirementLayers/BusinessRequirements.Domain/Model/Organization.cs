using BusinessRequirements.Domain.Common;

namespace BusinessRequirements.Domain.Model
{
    public class Organization : AggregateRoot<long>
    {
        public virtual string Name { get; private set; }

        protected Organization()
        {

        }
        private Organization(string name)
        {
            Name = name;
        }

        public static Organization CreateInstance(string name)
        {
            return new(name);
        }
    }
}
