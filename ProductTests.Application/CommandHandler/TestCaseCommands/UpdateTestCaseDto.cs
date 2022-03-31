using ProductTests.Domain.Model.TestCaseAggregate;
using System.Collections.Generic;

namespace ProductTests.Application.CommandHandler.TestCaseCommands
{
    public sealed class UpdateTestCaseDto
    {
        public string Title { get; set; }
        public string Preconditions { get; set; }
        public List<UpdateTestStepDto> TestSteps { get; set; }
    }
    public sealed class UpdateTestStepDto
    {
        public long Id { get; set; }
        public string Action { get; set; }
        public string ExpectedResult { get; set; }
        public TestStep ToTestStep(long id, long stepNo)
        {
            return TestStep.CreateInstance(id, stepNo, Action, ExpectedResult);
        }
    }
}
