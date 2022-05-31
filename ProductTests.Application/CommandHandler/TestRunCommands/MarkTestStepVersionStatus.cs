using ProductTests.Domain.Model.TestCaseVersionAggregate;

namespace ProductTests.Application.CommandHandler.TestRunCommands
{
    public sealed class MarkTestStepVersionStatus
    {
        public long Id { get; set; }
        public TestStepResult ResultStatus { get; set; }
    }
}
