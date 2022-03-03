using System.Collections.Generic;

namespace ProductDocumentations.Application.QueryHandlers
{
    public sealed class ProductDocumentationsQueryResult
    {
        public long Id { get; set; }
        public long? ParentId { get; set; }
        public List<GetProductDocumentationDto> ChildDocumentations { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long OrderNumber { get; set; }

        public GetProductDocumentationDto ToDto()
        {
            return new GetProductDocumentationDto(Id, ChildDocumentations, Title, ParentId, OrderNumber);
        }

        public GetProductDocumentationDetailsDto ToDetailsDto(long level, string index)
        {
            return new GetProductDocumentationDetailsDto(Id, level, Title, ParentId, Description, index, OrderNumber);
        }
    }

    public sealed class GetProductDocumentationDto
    {
        public long Id { get; set; }
        public List<GetProductDocumentationDto> ChildDocumentations { get; set; }
        public string Title { get; set; }
        public long? ParentId { get; set; }
        public string Index { get; set; }
        public long OrderNumber { get; set; }

        public GetProductDocumentationDto(long id, List<GetProductDocumentationDto> childDocumentations,
            string title, long? parentId, long orderNumber)
        {
            Id = id;
            ChildDocumentations = childDocumentations;
            Title = title;
            ParentId = parentId;
            OrderNumber = orderNumber;
        }
    }

    public sealed class GetProductDocumentationDetailsDto
    {
        public long Id { get; set; }
        public long Level { get; set; }
        public string Title { get; set; }
        public long? ParentId { get; set; }
        public string Description { get; set; }
        public string Index { get; set; }
        public long OrderNumber { get; set; }
        public GetProductDocumentationDetailsDto(long id, long level, string title, long? parentId, string description, string index, long orderNumber)
        {
            Id = id;
            Level = level;
            Title = title;
            ParentId = parentId;
            Description = description;
            Index = index;
            OrderNumber = orderNumber;
        }
    }
}
