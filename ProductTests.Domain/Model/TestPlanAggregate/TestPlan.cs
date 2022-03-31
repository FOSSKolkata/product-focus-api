using ProductTests.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductTests.Domain.Model.TestPlanAggregate
{
    public class TestPlan : AggregateRoot<long>, ISoftDeletable
    {
        public virtual string Name { get; private set; }
        public virtual long ProductId { get; private set; }
        public virtual long? SprintId { get; private set; }
        public virtual TestTypeEnum TestType { get; private set; }
        public virtual long ProductDocumentationId { get; private set; }
        public virtual long? WorkItemId { get; private set; }
        private readonly IList<TestSuite> _testSuites = new List<TestSuite>();
        public virtual IReadOnlyList<TestSuite> TestSuites => _testSuites.ToList();
        public virtual bool IsDeleted { get; set; }
        public virtual DateTime DeletedOn { get; set; }
        public virtual string DeletedBy { get; set; }

        protected TestPlan()
        {

        }

        private TestPlan(string name, long productId, long? sprintId, TestTypeEnum testType, long productDocumentationId, long? workItemId)
        {
            Name = name;
            ProductId = productId;
            SprintId = sprintId;
            TestType = testType;
            ProductDocumentationId = productDocumentationId;
            WorkItemId = workItemId;
        }

        public void ChangeSprint(long? sprintId)
        {
            SprintId = sprintId;
        }
        public void DeleteTestSuite(long suiteId, string userId)
        {
            var deleteSuite = this.TestSuites.Where(x => x.Id == suiteId).SingleOrDefault();
            deleteSuite.Delete(userId);
            //this._testSuites.Remove(deleteSuite); // Will be deleted permanently
        }
        public void Delete(string userId)
        {
            DeletedBy = userId;
            DeletedOn = DateTime.Now;
            IsDeleted = true;
        }

        public static TestPlan CreateInstance(string name, long productId, long? sprintId, TestTypeEnum testType, long productDocumentationId, long? workItemId)
        {
            TestPlan testPlan = new(name, productId, sprintId, testType, productDocumentationId, workItemId);
            return testPlan;
        }

        public virtual void AddTestSuite(string name, long testPlanId)
        {
            _testSuites.Add(TestSuite.CreateInstance(name, testPlanId));
        }
    }

    public enum TestTypeEnum
    {
        RegressionTest = 1,
        WorkItemBased = 2
    }
}
