using ProductFocus.Domain.Model;

namespace ProductFocusApi.Dtos
{
    public sealed class GetWorkItemDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public WorkItemType WorkItemType { get; set; }
        public long workCompletionPercentage { get; set; }
    }
}
