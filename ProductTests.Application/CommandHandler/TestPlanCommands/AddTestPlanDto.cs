using ProductTests.Domain.Model.TestPlanAggregate;

namespace ProductTests.Application.CommandHandler.TestPlanCommands
{
    public sealed class AddTestPlanDto
    {
        public long ProductId { get; set; }
        public long? SprintId { get; set; }
        public TestTypeEnum TestType { get; set; }
        public long ProductDocumentationId { get; set; }
        public long? WorkItemId { get; set; }
        public string Title { get; set; }
    }
}
