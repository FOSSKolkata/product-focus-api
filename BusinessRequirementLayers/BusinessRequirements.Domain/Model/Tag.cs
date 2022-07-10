using BusinessRequirements.Domain.Common;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessRequirements.Domain.Model
{
    public class Tag : AggregateRoot<long>, ISoftDeletable
    {
        public string Name { get; private set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedOn { get; set; }
        public string DeletedBy { get; set; }
        public long ProductId { get; set; }
        protected Tag()
        {
            // this protected constructor is for lazy loading to work
        }
        private Tag(string name, long productId)
        {
            Name = name;
            ProductId = productId;
        }
        public static Result<Tag> CreateInstance(string name, long productId)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Result.Failure<Tag>("Name can't be null or empty.");
            }
            Tag tag = new(name, productId);
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
