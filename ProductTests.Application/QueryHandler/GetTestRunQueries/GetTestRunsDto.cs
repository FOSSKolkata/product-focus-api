using ProductTests.Domain.Model.TestCaseVersionAggregate;
using ProductTests.Domain.Model.TestPlanAggregate;

namespace ProductTests.Application.QueryHandler.GetTestRunQueries
{
    public class GetTestRunsDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public TestTypeEnum TestType { get; set; }
        public long TotalTestSuites { get; set; }
        public long TotalTestCases { get; set; }
        public long TotalPassedCases { get; set; }
        public long TotalFailedCases { get; set; }
    }
    public class GetTestSuiteDto
    {
        public long Id { get; set; }
        public long PlanId { get; set; }
        public string Title { get; set; }
    }
    public class GetTestCaseDto
    {
        public long Id { get; set; }
        public long SuiteId { get; set; }
        public string Title { get; set; }
        public TestCaseResult ResultStatus { get; set; }
    }
}
