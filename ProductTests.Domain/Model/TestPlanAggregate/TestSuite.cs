using ProductTests.Domain.Common;
using ProductTests.Domain.Model.TestCaseAggregate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductTests.Domain.Model.TestPlanAggregate
{
    public class TestSuite : Entity<long>, ISoftDeletable
    {
        public virtual string Name { get; private set; }
        public virtual long TestPlanId { get; private set; }
        public virtual TestPlan TestPlan { get; private set; }
        public virtual long OrderNo { get; private set; }
        private readonly IList<TestSuiteTestCaseMapping> _testSuiteTestCaseMappings = new List<TestSuiteTestCaseMapping>();
        public virtual IReadOnlyList<TestSuiteTestCaseMapping> TestSuiteTestCaseMappings => _testSuiteTestCaseMappings.ToList();
        public virtual bool IsDeleted { get; set; }
        public virtual DateTime DeletedOn { get; set; }
        public virtual string DeletedBy { get; set; }

        protected TestSuite()
        {

        }

        private TestSuite(string name, long testPlanId)
        {
            Name = name;
            TestPlanId = testPlanId;
        }
        internal void AddTestCaseToTestSuiteMapping(TestCase testCase)
        {
            _testSuiteTestCaseMappings.Add(TestSuiteTestCaseMapping.CreateInstance(this, testCase));
        }

        internal void DeleteTestSuiteTestCaseMapping(long testCaseId, string userId)
        {
            TestSuiteTestCaseMapping testSuiteTestCaseMapping = TestSuiteTestCaseMappings.Where(x => x.TestCase.Id == testCaseId).SingleOrDefault();
            testSuiteTestCaseMapping.Delete(userId);
        }
        internal void Delete(string userId)
        {
            IsDeleted = true;
            DeletedOn = DateTime.Now;
            DeletedBy = userId;
        }
        internal void UpdateOrder(long orderNo)
        {
            OrderNo = orderNo;
        }
        internal static TestSuite CreateInstance(string name, long testPlanId)
        {
            return new(name, testPlanId);
        }
    }
}
