using ProductTests.Domain.Model.TestCaseAggregate;
using System.Collections.Generic;

namespace ProductTests.Application.CommandHandler.TestCaseCommands
{
    public sealed class AddTestCaseDto
    {
        public string Title { get; set; }
        public string Preconditions { get; set; }
        public long SuiteId { get; set; }
        public List<AddTestStepDto> TestSteps { get; set; }
    }
    public sealed class AddTestStepDto
    {
        public string Action { get; set; }
        public string ExpectedResult { get; set; }
        /*public TestStep ToTestStep(long id, long stepNo)
        {
            return TestStep.CreateInstance(id, stepNo, Action, ExpectedResult);
        }*/
        public TestStep ToTestStep(long stepNo)
        {
            return TestStep.CreateInstance(stepNo, Action, ExpectedResult);
        }
    }
}
