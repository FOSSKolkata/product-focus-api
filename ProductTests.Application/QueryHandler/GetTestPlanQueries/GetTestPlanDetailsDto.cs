using ProductTests.Domain.Model.TestPlanAggregate;
using System.Collections.Generic;

namespace ProductTests.Application.QueryHandler.GetTestPlanQueries
{
    public sealed class GetTestPlanDetailsDto
    {
        public long TestPlanId { get; set; }
        public string TestPlanTitle { get; set; }
        public TestTypeEnum TestType { get; set; }
        public List<TestSuiteDetailsDto> TestSuites { get; set; }
    }
    public sealed class TestSuiteDetailsDto
    {
        public long TestSuiteId { get; set; }
        public string TestSuiteTitle { get; set; }
        public List<TestCaseDetailsDto> TestCases { get; set; }
    }
    public sealed class TestCaseDetailsDto
    {
        public long TestSuiteId { get; set; }
        public long TestCaseId { get; set; }
        public string TestCaseTitle { get; set; }
        public string Preconditions { get; set; }
        public List<TestStepDetailsDto> TestSteps { get; set; }
    }
    public sealed class TestStepDetailsDto
    {
        public long TestCaseId { get; set; }
        public long TestStepId { get; set; }
        public long StepNo { get; set; }
        public string Action { get; set; }
        public string ExpectedResult { get; set; }
    }
}
