using ProductTests.Domain.Model.TestPlanAggregate;

namespace ProductTests.Application.QueryHandler.GetTestPlanQueries
{
    public sealed class GetTestPlansDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string SprintTitle { get; set; }
        public long SuiteCount { get; set; }
        public TestTypeEnum TestType { get; set; }
    }

    public sealed class GetTestPlanSuiteIdDto
    {
        public long Id { get; set; }
        public long TestPlanId { get; set; }
    }
}
