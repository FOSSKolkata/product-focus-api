using ProductTests.Domain.Common;
using ProductTests.Domain.Model.TestCaseVersionAggregate;
using ProductTests.Domain.Model.TestPlanAggregate;

namespace ProductTests.Domain.Model.TestPlanVersionAggregate
{
    public class TestSuiteTestCaseMappingVersion : Entity<long>
    {
        public virtual TestSuiteVersion TestSuiteVersion { get; private set; }
        public virtual TestCaseVersion TestCaseVersion { get; private set; }
        protected TestSuiteTestCaseMappingVersion()
        {

        }
        private TestSuiteTestCaseMappingVersion(TestSuiteTestCaseMapping mapping, TestSuiteVersion testSuiteVersion)
        {
            TestSuiteVersion = testSuiteVersion;
            TestCaseVersion = TestCaseVersion.CreateInstance(mapping.TestCase);
        }
        public static TestSuiteTestCaseMappingVersion CreateInstance(TestSuiteVersion testSuiteVersion, TestSuiteTestCaseMapping mapping)
        {
            return new TestSuiteTestCaseMappingVersion(mapping, testSuiteVersion);
        }
    }
}
