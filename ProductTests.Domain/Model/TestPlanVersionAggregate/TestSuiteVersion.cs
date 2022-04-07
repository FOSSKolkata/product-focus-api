using ProductTests.Domain.Common;
using ProductTests.Domain.Model.TestPlanAggregate;
using System.Collections.Generic;
using System.Linq;

namespace ProductTests.Domain.Model.TestPlanVersionAggregate
{
    public class TestSuiteVersion : Entity<long>
    {
        public virtual string Name { get; private set; }
        public virtual long TestPlanVersionId { get; private set; }

        private readonly IList<TestSuiteTestCaseMappingVersion> _testSuiteTestCaseMappings = new List<TestSuiteTestCaseMappingVersion>();
        public virtual IReadOnlyList<TestSuiteTestCaseMappingVersion> TestSuiteTestCaseMappings => _testSuiteTestCaseMappings.ToList();
        public virtual TestPlanVersion TestPlanVersion { get; private set; }
        protected TestSuiteVersion()
        {

        }
        private TestSuiteVersion(TestSuite testSuite, long testPlanVersionId)
        {
            Name = testSuite.Name;
            TestPlanVersionId = testPlanVersionId;
            foreach(TestSuiteTestCaseMapping mapping in testSuite.TestSuiteTestCaseMappings)
            {
                _testSuiteTestCaseMappings.Add(TestSuiteTestCaseMappingVersion.CreateInstance(this,mapping));
            }
        }

        public static TestSuiteVersion CreateInstance(TestSuite testSuite, long testPlanVersionId)
        {
            return new TestSuiteVersion(testSuite, testPlanVersionId);
        }
    }
}
