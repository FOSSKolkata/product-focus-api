using CSharpFunctionalExtensions;
using ProductTests.Domain.Model.TestPlanAggregate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductTests.Domain.Model.TestRunAggregate
{
    public class TestPlanVersion : Entity<long>
    {
        public virtual string Name { get; private set; }
        public virtual TestTypeEnum TestType { get; private set; }
        public virtual long ProductDocumentationId { get; private set; }
        public virtual long? WorkItemId { get; private set; }
        private readonly IList<TestSuiteVersion> _testSuitesVersion = new List<TestSuiteVersion>();
        public virtual IReadOnlyList<TestSuiteVersion> TestSuitesVersion => _testSuitesVersion.ToList();
        protected TestPlanVersion()
        {

        }
        internal void AddTestSuiteVersion(TestSuiteVersion testSuiteVersion)
        {
            _testSuitesVersion.Add(testSuiteVersion);
        }
        private TestPlanVersion(TestPlan testPlan)
        {
            Name = testPlan.Name;
            TestType = testPlan.TestType;
            ProductDocumentationId = testPlan.ProductDocumentationId;
            WorkItemId = testPlan.WorkItemId;
            foreach(TestSuite testSuite in testPlan.TestSuites)
            {
                _testSuitesVersion.Add(TestSuiteVersion.CreateInstance(testSuite, Id));
            }
        }
        internal void IncludeTestCase(long testCaseId, bool isSelected)
        {
            foreach(var testSuite in TestSuitesVersion)
            {
                testSuite.IncludeTestCase(testCaseId, isSelected);
            }
        }

        internal void UpdateTestStep(long id, TestStepResult resultStatus)
        {
            foreach(var testSuite in TestSuitesVersion)
            {
                testSuite.UpdateTestStep(id, resultStatus);
            }
        }

        internal void UpdateResultStatus(long id, TestCaseResult resultStatus)
        {
            foreach(var testSuite in TestSuitesVersion)
            {
                testSuite.UpdateResultStatus(id, resultStatus);
            }
        }

        internal static TestPlanVersion CreateInstance(TestPlan testPlan)
        {
            return new TestPlanVersion(testPlan);
        }
    }
}
