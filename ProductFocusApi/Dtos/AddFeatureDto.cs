namespace ProductFocus.Dtos
{
    public sealed class AddFeatureDto
    {
        public string Title { get; set; }        
        public string WorkItemType { get; set; }
        public long? SprintId { get; set; }
    }
}
