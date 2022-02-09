namespace ProductDocumentations.Application
{
    public sealed class UpdateDocumentationDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public UpdateDocumentationFieldName FieldName { get; set; }
    }
    public enum UpdateDocumentationFieldName
    {
        Title = 1,
        Description = 2
    }
}
