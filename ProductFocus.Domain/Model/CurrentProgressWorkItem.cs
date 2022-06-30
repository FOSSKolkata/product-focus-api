using ProductFocus.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Model
{
    public class CurrentProgressWorkItem : AggregateRoot<long>, ISoftDeletable
    {
        public long ProductId { get; private set; }
        public long WorkItemId { get; private set; }
        public long UserId { get; private set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedOn { get; set; }
        public string DeletedBy { get; set; }
        protected CurrentProgressWorkItem()
        {

        }

        private CurrentProgressWorkItem(long productId, long workItemId, long userId)
        {
            ProductId = productId;
            WorkItemId = workItemId;
            UserId = userId;
        }

        public static CurrentProgressWorkItem CreateInstance(long productId, long workItemId, long userId)
        {
            return new(productId, workItemId, userId);
        }

        public void Delete(string deletedBy)
        {
            DeletedBy = deletedBy;
            DeletedOn = DateTime.Now;
            IsDeleted = true;
            LastModifiedBy = deletedBy;
            LastModifiedOn = DateTime.Now;
        }
    }
}
