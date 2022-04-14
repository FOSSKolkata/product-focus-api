using ProductTests.Domain.Model.TestPlanAggregate;

namespace ProductTests.Application.QueryHandler.GetTestPlanQueries
{
    public sealed class GetTestPlansDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long? SprintId { get; set; }
        public long SuiteCount { get; set; }
        public TestTypeEnum TestType { get; set; }
    }
}
