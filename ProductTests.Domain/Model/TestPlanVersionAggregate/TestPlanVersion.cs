using ProductTests.Domain.Common;
using ProductTests.Domain.Model.TestPlanAggregate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductTests.Domain.Model.TestPlanVersionAggregate
{
    public class TestPlanVersion : AggregateRoot<long>, ISoftDeletable
    {
        public virtual string Name { get; private set; }
        public virtual long TestPlanId { get; private set; }
        public virtual TestPlan TestPlan { get; private set; }
        public virtual long ProductId { get; private set; }
        public virtual long? SprintId { get; private set; }
        public virtual TestTypeEnum TestType { get; private set; }
        public virtual long ProductDocumentationId { get; private set; }
        public virtual long? WorkItemId { get; private set; }
        private readonly IList<TestSuiteVersion> _testSuitesVersion = new List<TestSuiteVersion>();
        public virtual IReadOnlyList<TestSuiteVersion> TestSuitesVersion => _testSuitesVersion.ToList();
        public virtual bool IsDeleted { get; set; }
        public virtual DateTime DeletedOn { get; set; }
        public virtual string DeletedBy { get; set; }
        public virtual Status RunningStatus { get; set; }
        protected TestPlanVersion()
        {

        }
        public void ChangeRunningStatus(Status runningStatus)
        {
            RunningStatus = runningStatus;
        }
        private TestPlanVersion(TestPlan testPlan)
        {
            Name = testPlan.Name;
            TestPlanId = testPlan.Id;
            TestPlan = testPlan;
            ProductId = testPlan.ProductId;
            SprintId = testPlan.SprintId;
            TestType = testPlan.TestType;
            ProductDocumentationId = testPlan.ProductDocumentationId;
            WorkItemId = testPlan.WorkItemId;
            foreach(TestSuite testSuite in testPlan.TestSuites)
            {
                _testSuitesVersion.Add(TestSuiteVersion.CreateInstance(testSuite, Id));
            }
        }
        public static TestPlanVersion CreateInstance(TestPlan testPlan)
        {
            return new TestPlanVersion(testPlan);
        }
        public enum Status
        {
            Completed = 1,
            Incompleted = 2
        }
    }
}
