using ProductTests.Domain.Common;
using ProductTests.Domain.Model.TestPlanAggregate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductTests.Domain.Model.TestRunAggregate
{
    public class TestSuiteVersion : Entity<long>
    {
        public virtual string Name { get; private set; }
        public virtual long TestPlanVersionId { get; private set; }

        private readonly IList<TestCaseVersion> _testCaseVersions = new List<TestCaseVersion>();
        public virtual IReadOnlyList<TestCaseVersion> TestCaseVersions => _testCaseVersions.ToList();
        public virtual TestPlanVersion TestPlanVersion { get; private set; }
        protected TestSuiteVersion()
        {

        }

        private TestSuiteVersion(TestSuite testSuite, long testPlanVersionId)
        {
            Name = testSuite.Name;
            TestPlanVersionId = testPlanVersionId;
            foreach(TestSuiteTestCaseMapping testSuiteTestCaseMapping in testSuite.TestSuiteTestCaseMappings)
            {
                _testCaseVersions.Add(TestCaseVersion.CreateInstance(testSuiteTestCaseMapping.TestCase));
            }
        }
        internal void IncludeTestCase(long testCaseId, bool status)
        {
            TestCaseVersions.Where(x => x.Id == testCaseId)
                .SingleOrDefault()?.IncludeTestCase(status);
        }
        internal void UpdateResultStatus(long testCaseId, TestCaseResult resultStatus)
        {
            TestCaseVersions.Where(x => x.Id == testCaseId)
                .SingleOrDefault()?.UpdateResultStatus(resultStatus);
        }
        internal static TestSuiteVersion CreateInstance(TestSuite testSuite, long testPlanVersionId)
        {
            return new TestSuiteVersion(testSuite, testPlanVersionId);
        }

        internal void UpdateTestStep(long id, TestStepResult resultStatus)
        {
            foreach(var testCase in TestCaseVersions)
            {
                testCase.UpdateTestStep(id, resultStatus);
            }    
        }

        internal bool IsTestSuiteExecutionCompleted()
        {
            bool isCompleted = true;
            foreach(var testCase in TestCaseVersions)
            {
                isCompleted &= testCase.IsTestCaseExecutionCompleted();
            }
            return isCompleted;
        }
    }
}
