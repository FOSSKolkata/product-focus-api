using ProductTests.Domain.Model.TestPlanAggregate;
using System.Collections.Generic;

namespace ProductTests.Application.QueryHandler.GetTestPlanQueries
{
    public sealed class GetTestPlanDetailsDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public TestTypeEnum TestType { get; set; }
        public List<TestSuiteDetailsDto> TestSuites { get; set; }
    }
    public sealed class TestSuiteDetailsDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public List<TestCaseDetailsDto> TestCases { get; set; }
    }
    public sealed class TestCaseDetailsDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Preconditions { get; set; }
        public List<TestStepDetailsDto> TestSteps { get; set; }
    }
    public sealed class TestStepDetailsDto
    {
        public long Id { get; set; }
        public long StepNo { get; set; }
        public string Action { get; set; }
        public string ExpectedResult { get; set; }
    }
}
