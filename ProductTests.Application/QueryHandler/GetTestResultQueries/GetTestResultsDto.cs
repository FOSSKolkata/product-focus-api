using ProductTests.Domain.Model.TestPlanAggregate;
using ProductTests.Domain.Model.TestRunAggregate;
using System;

namespace ProductTests.Application.QueryHandler.GetTestResultQueries
{
    public sealed class GetTestResultDto
    {
        public long TestRunId { get; set; }
        public long SprintId { get; set; }
        public Status RunningStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Title { get; set; }
        public long TestPlanVersionId { get; set; }
        public TestTypeEnum TestType { get; set; }
        public long Passed { get; set; }
        public long Failed { get; set; }
        public long TestCasesCount { get; set; }
    }

    public sealed class GetTestResultDetailsDto
    {
        public long TestRunId { get; set; }
        public long TestPlanId { get; set; }
        public string TestSuiteId { get; set; }
        public TestCaseResult ResultStatus { get; set; }
        public long TestCaseCount { get; set; }
    }
}
