using ProductTests.Domain.Common;
using ProductTests.Domain.Model.TestPlanAggregate;

namespace ProductTests.Domain.Model.TestPlanVersionAggregate
{
    public class TestSuiteVersion : Entity<long>
    {
        public virtual string Name { get; private set; }
        public virtual long TestPlanVersionId { get; private set; }
        public virtual TestPlanVersion TestPlanVersion { get; private set; }
        protected TestSuiteVersion()
        {

        }
        private TestSuiteVersion(TestSuite testSuite, long testPlanVersionId)
        {
            Name = testSuite.Name;
            TestPlanVersionId = testPlanVersionId;
        }

        public static TestSuiteVersion CreateInstance(TestSuite testSuite, long testPlanVersionId)
        {
            return new TestSuiteVersion(testSuite, testPlanVersionId);
        }
    }
}
