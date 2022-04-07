using ProductTests.Domain.Common;
using ProductTests.Domain.Model.TestCaseAggregate;
using System;

namespace ProductTests.Domain.Model.TestPlanAggregate
{
    public class TestSuiteTestCaseMapping : Entity<long>, ISoftDeletable
    {
        public virtual TestSuite TestSuite { get; set; }
        public virtual TestCase TestCase { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual DateTime DeletedOn { get; set; }
        public virtual string DeletedBy { get; set; }

        protected TestSuiteTestCaseMapping()
        {

        }
        private TestSuiteTestCaseMapping(TestSuite testSuite, TestCase testCase)
        {
            TestSuite = testSuite;
            TestCase = testCase;
        }
        internal void Delete(string userId)
        {
            IsDeleted = true;
            DeletedOn = DateTime.Now;
            DeletedBy = userId;
        }
        public static TestSuiteTestCaseMapping CreateInstance(TestSuite testSuite, TestCase testCase)
        {
            return new TestSuiteTestCaseMapping(testSuite, testCase);
        }
    }
}
