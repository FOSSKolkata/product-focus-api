namespace ProductDocumentations.Application.Dtos
{
    public sealed class ProductDocumentationDto
    {
        public long? ParentId { get; set; }
        public long ProductId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
