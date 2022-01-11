using Common;
using CSharpFunctionalExtensions;
using System;

namespace ProductFocus.Domain.Model
{
    public class Tag : AggregateRoot<long>, ISoftDeletable
    {
        public virtual string Name { get; private set; }
        public virtual TagCategory TagCategory { get; private set; }
        public virtual long ProductId { get; private set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedOn { get; set; }
        public string DeletedBy { get; set; }

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

        public void Delete(string userId)
        {
            IsDeleted = true;
            DeletedBy = userId;
            DeletedOn = DateTime.Now;
        }
    }
}
