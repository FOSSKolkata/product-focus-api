using ProductDocumentations.Domain.Model;
using System;

namespace ProductDocumentations.Application
{
    public sealed class ProductDocumentationDto
    {
        public long? ParentId { get; set; }
        public long ProductId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ProductDocumentationDto(long? parentId, long productId, string title, string description)
        {
            ParentId = parentId;
            ProductId = productId;
            Title = title;
            Description = description;
        }
    }
}
