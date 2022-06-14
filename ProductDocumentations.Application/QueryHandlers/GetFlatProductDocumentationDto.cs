namespace ProductDocumentations.Application.QueryHandlers
{
    public class GetFlatProductDocumentationDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long ProductId { get; set; }
        public long Descripttion { get; set; }

    }
}
