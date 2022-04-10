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
        private TestSuiteTestCaseMappingVersion(TestCaseVersion testCaseVersion, TestSuiteVersion testSuiteVersion)
        {
            TestSuiteVersion = testSuiteVersion;
            TestCaseVersion = testCaseVersion;
        }
        public static TestSuiteTestCaseMappingVersion CreateInstance(TestSuiteVersion testSuiteVersion, TestCaseVersion testCaseVersion)
        {
            return new TestSuiteTestCaseMappingVersion(testCaseVersion, testSuiteVersion);
        }
    }
}
