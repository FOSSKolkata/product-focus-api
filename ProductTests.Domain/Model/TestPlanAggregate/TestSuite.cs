using ProductTests.Domain.Common;
using System;

namespace ProductTests.Domain.Model.TestPlanAggregate
{
    public class TestSuite : Entity<long>, ISoftDeletable
    {
        public virtual string Name { get; private set; }
        public virtual long TestPlanId { get; private set; }
        public virtual TestPlan TestPlan {get; private set;}
        public bool IsDeleted { get; set; }
        public DateTime DeletedOn { get; set; }
        public string DeletedBy { get; set; }

        protected TestSuite()
        {

        }

        private TestSuite(string name, long testPlanId)
        {
            Name = name;
            TestPlanId = testPlanId;
        }

        internal void Delete(string userId)
        {
            IsDeleted = true;
            DeletedOn = DateTime.Now;
            DeletedBy = userId;
        }

        internal static TestSuite CreateInstance(string name, long testPlanId)
        {
            return new(name, testPlanId);
        }
    }
}
