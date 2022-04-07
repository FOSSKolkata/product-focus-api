using ProductTests.Domain.Common;
using ProductTests.Domain.Model.TestCaseAggregate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductTests.Domain.Model.TestCaseVersionAggregate
{
    public class TestCaseVersion : AggregateRoot<long>, ISoftDeletable
    {
        public virtual string Title { get; private set; }
        public virtual string Preconditions { get; private set; }
        private readonly IList<TestStepVersion> _testStepsVersion = new List<TestStepVersion>();
        public virtual IReadOnlyList<TestStepVersion> TestStepsVersion => _testStepsVersion.ToList();
        public virtual bool IsIncluded { get; private set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedOn { get; set; }
        public string DeletedBy { get; set; }
        protected TestCaseVersion()
        {

        }
        private TestCaseVersion(TestCase testCase)
        {
            Title = testCase.Title;
            Preconditions = testCase.Preconditions;
            foreach(TestStep testStep in testCase.TestSteps)
            {
                _testStepsVersion.Add(TestStepVersion.CreateInstance(testStep, Id));
            }
        }
        public void IncludeTestCast(bool isIncluded)
        {
            IsIncluded = isIncluded;
        }
        public static TestCaseVersion CreateInstance(TestCase testCase)
        {
            return new TestCaseVersion(testCase);
        }
        public bool IsPassed
        {
            get
            {
                foreach (TestStepVersion testStep in TestStepsVersion)
                {
                    if (testStep.GetResultStatus() == TestStepResult.Failed)
                        return false;
                }
                return true;
            }
        }
    }
}
