using Common;
using CSharpFunctionalExtensions;

namespace ProductFocus.Domain.Model
{
    public class Tag : AggregateRoot<long>
    {
        public virtual string Name { get; private set; }
        public virtual TagCategory TagCategory { get; private set; }
        public virtual long ProductId { get; private set; }
        protected Tag()
        {
            // this protected constructor is for lazy loading to work
        }
        private Tag(string name, long productId, TagCategory tagCategory)
        {
            Name = name;
            TagCategory = tagCategory;
            ProductId = productId;
        }
        public static Result<Tag> CreateInstance(string name, long productId, TagCategory tagCategory)
        {
            if(string.IsNullOrEmpty(name))
            {
                return Result.Failure<Tag>("Name can't be null or empty.");
            }
            Tag tag = new(name, productId, tagCategory);
            return tag;
        }
    }
}
