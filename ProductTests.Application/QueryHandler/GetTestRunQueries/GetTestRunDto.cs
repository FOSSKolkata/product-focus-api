using ProductTests.Domain.Model.TestPlanAggregate;
using ProductTests.Domain.Model.TestRunAggregate;
using System.Collections.Generic;

namespace ProductTests.Application.QueryHandler.GetTestRunQueries
{
    public class GetTestRunDto
    {
        // Test Plan details will be stored
        public long Id { get; set; }
        public long PlanId { get; set; }
        public string Title { get; set; }
        public TestTypeEnum TestType { get; set; }
        public List<GetTestRunSuiteDto> TestSuites { get; set; }
    }
    public class GetTestRunSuiteDto
    {
        public long Id { get; set; }
        public long TestPlanId { get; set; }
        public string Title { get; set; }
        public List<GetTestRunCaseDto> TestCases { get; set; }
    }
    public class GetTestRunCaseDto
    {
        public long Id { get; set; }
        public long TestSuiteId { get; set; }
        public string Title { get; set; }
        public bool IsIncluded { get; set; }
        public TestCaseResult ResultStatus { get; set; }
        public List<GetTestRunStepDto> TestSteps { get; set; }
    }
    public class GetTestRunStepDto
    {
        public long Id { get; set; }
        public long TestCaseId { get; set; }
        public virtual long StepNo { get; private set; }
        public virtual string Action { get; private set; }
        public virtual string ExpectedResult { get; private set; }
        public virtual TestStepResult ResultStatus { get; private set; }
    }
}
