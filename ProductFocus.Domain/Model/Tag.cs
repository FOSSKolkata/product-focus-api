using Common;
using CSharpFunctionalExtensions;

namespace ProductFocus.Domain.Model
{
    public class Tag : AggregateRoot<long>
    {
        public virtual string Name { get; set; }
        public virtual TagCategory TagCategory { get; set; }
        protected Tag()
        {
            // this protected constructor is for lazy loading to work
        }
        private Tag(string name, TagCategory tagCategory)
        {
            Name = name;
            TagCategory = tagCategory;
        }
        public static Result<Tag> CreateInstance(string name, TagCategory tagCategory)
        {
            if(string.IsNullOrEmpty(name))
            {
                return Result.Failure<Tag>("Name can't be null or empty.");
            }
            Tag tag = new(name, tagCategory);
            return tag;
        }
    }
}
