using ProductTests.Domain.Common;
using ProductTests.Domain.Model.TestCaseAggregate;

namespace ProductTests.Domain.Model.TestRunAggregate
{
    public class TestStepVersion : Entity<long>
    {
        public virtual long TestCaseVersionId { get; private set; }
        public virtual TestCaseVersion TestCaseVersion { get; private set; }
        public virtual long StepNo { get; private set; }
        public virtual string Action { get; private set; }
        public virtual string ExpectedResult { get; private set; }
        public virtual TestStepResult ResultStatus { get; private set; }
        protected TestStepVersion()
        {

        }
        private TestStepVersion(TestStep testStep, long testCaseVersionId)
        {
            TestCaseVersionId = testCaseVersionId;
            StepNo = testStep.StepNo;
            Action = testStep.Action;
            ExpectedResult = testStep.ExpectedResult;
        }
        internal void UpdateRunStatus(TestStepResult resultStatus)
        {
            ResultStatus = resultStatus;
        }
        public TestStepResult GetResultStatus()
        {
            return ResultStatus;
        }
        internal static TestStepVersion CreateInstance(TestStep testStep, long testCaseVersionId)
        {
            return new TestStepVersion(testStep, testCaseVersionId);
        }
    }
    public enum TestStepResult
    {
        Success = 1,
        Failed = 2,
        Pending = 3,
    }
}
