﻿using ProductDocumentations.Domain.Common;
using System.Collections.Generic;
using System.Linq;

namespace ProductDocumentations.Domain.Model
{
    public class ProductDocumentation : AggregateRoot<long>
    {
        public virtual long? ParentId { get; private set; }
        public virtual long ProductId { get; private set; }
        public virtual string Title { get; private set; }
        public virtual string Description { get; private set; }
        public readonly IList<ProductDocumentationAttachment> _productDocumentationAttachments = new List<ProductDocumentationAttachment>();
        public virtual IReadOnlyList<ProductDocumentationAttachment> ProductDocumentationAttachments => _productDocumentationAttachments.ToList();
        public virtual long OrderNumber { get; private set; }
        
        protected ProductDocumentation()
        {

        }

        private ProductDocumentation(long? parentId, long productId,
            string title, string description)
        {
            ParentId = parentId;
            ProductId = productId;
            Title = title;
            Description = description;
        }

        public static ProductDocumentation CreateInstance(long? parentId, long productId,
            string title, string description)
        {
            ProductDocumentation productDocumentation = new(parentId, productId, title, description);
            return productDocumentation;
        }

        public virtual void AddAttachment(string name, string uri, string fileName)
        {
            _productDocumentationAttachments.Add(ProductDocumentationAttachment.CreateInstance(Id, name, uri, fileName));
        }

        public virtual void UpdateTitle(string title)
        {
            Title = title;
        }

        public virtual void UpdateDescription(string description)
        {
            Description = description;
        }
    }
}
