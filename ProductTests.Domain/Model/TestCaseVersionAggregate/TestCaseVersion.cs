using ProductTests.Domain.Common;
using ProductTests.Domain.Model.TestCaseAggregate;
using ProductTests.Domain.Model.TestPlanVersionAggregate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductTests.Domain.Model.TestCaseVersionAggregate
{
    public class TestCaseVersion : AggregateRoot<long>, ISoftDeletable
    {
        public virtual long TestSuiteVersionId { get; private set; }
        public virtual TestSuiteVersion TestSuiteVersion { get; private set; }
        public virtual string Title { get; private set; }
        public virtual string Preconditions { get; private set; }

        private readonly IList<TestStepVersion> _testStepsVersion = new List<TestStepVersion>();
        public virtual IReadOnlyList<TestStepVersion> TestSteps => _testStepsVersion.ToList();
        public virtual bool IsIncluded { get; private set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedOn { get; set; }
        public string DeletedBy { get; set; }
        protected TestCaseVersion()
        {

        }
        private TestCaseVersion(TestCase testCase, long testSuiteVersionId, List<TestStep> testSteps)
        {
            TestSuiteVersionId = testSuiteVersionId;
            Title = testCase.Title;
            Preconditions = testCase.Preconditions;
            foreach(TestStep testStep in testSteps)
            {
                _testStepsVersion.Add(TestStepVersion.CreateInstance(testStep, Id));
            }
        }
        public void IncludeTestCast(bool isIncluded)
        {
            IsIncluded = isIncluded;
        }
        public static TestCaseVersion CreateInstance(TestCase testCase, long testSuiteVersionId, List<TestStep> testSteps)
        {
            return new TestCaseVersion(testCase, testSuiteVersionId, testSteps);
        }
    }
}
