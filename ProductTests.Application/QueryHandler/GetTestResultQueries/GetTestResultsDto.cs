using ProductTests.Domain.Model.TestPlanAggregate;

namespace ProductTests.Application.QueryHandler.GetTestResultQueries
{
    public sealed class GetTestResultsDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public TestTypeEnum TestType { get; set; }
        public long CaseCount { get; set; }
        public long CasesPassed { get; set; }
        public long CasesFailed { get; set; }
    }

    public sealed class GetTestSuiteResultDto
    {
        public long SuiteId { get; set; }
        public TestTypeEnum ResultStatus { get; set; }
        public long ResultCount { get; set; }
    }
}
