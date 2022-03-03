using System;

namespace ProductDocumentations.Application.CommandHandlers.AddProductDocumentation
{
    public sealed class AddProductDocumentationDto
    {
        public long? ParentId { get; set; }
        public long ProductId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public AddProductDocumentationDto(long? parentId, long productId, string title, string description)
        {
            ParentId = parentId;
            ProductId = productId;
            Title = title;
            Description = description;
        }
    }
}
