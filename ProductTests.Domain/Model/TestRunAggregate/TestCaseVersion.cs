using ProductTests.Domain.Common;
using ProductTests.Domain.Model.TestCaseAggregate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductTests.Domain.Model.TestRunAggregate
{
    public class TestCaseVersion : Entity<long>
    {
        public virtual string Title { get; private set; }
        public virtual string Preconditions { get; private set; }
        private readonly IList<TestStepVersion> _testStepsVersion = new List<TestStepVersion>();
        public virtual IReadOnlyList<TestStepVersion> TestStepsVersion => _testStepsVersion.ToList();
        public virtual bool IsIncluded { get; private set; }
        public virtual TestCaseResult ResultStatus { get; private set; }
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
            foreach (TestStep testStep in testCase.TestSteps)
            {
                _testStepsVersion.Add(TestStepVersion.CreateInstance(testStep, Id));
            }
        }
        internal void IncludeTestCase(bool isIncluded)
        {
            IsIncluded = isIncluded;
        }
        internal static TestCaseVersion CreateInstance(TestCase testCase)
        {
            return new TestCaseVersion(testCase);
        }
        internal void UpdateResultStatus(TestCaseResult resultStatus)
        {
            ResultStatus = resultStatus;
        }
        internal void UpdateTestStep(long id, TestStepResult resultStatus)
        {
            TestStepsVersion.Where(x => x.Id == id).SingleOrDefault()?
                .UpdateRunStatus(resultStatus);
        }
    }
    public enum TestCaseResult
    {
        Success = 1,
        Failed = 2,
        Pending = 3,
    }
}
