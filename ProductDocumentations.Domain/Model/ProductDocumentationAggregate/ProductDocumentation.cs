using Common;
using System.Collections.Generic;
using System.Linq;

namespace ProductDocumentations.Domain.Model
{
    public class ProductDocumentation : AggregateRoot<long>
    {
        public virtual long ParentId { get; set; }
        public virtual long ProductId { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public readonly IList<ProductDocumentationAttachment> _productDocumentationAttachments = new List<ProductDocumentationAttachment>();
        public virtual IReadOnlyList<ProductDocumentationAttachment> ProductdocumentationAttachments => _productDocumentationAttachments.ToList();
        
        protected ProductDocumentation()
        {

        }

        private ProductDocumentation(long parentId, long productId,
            string title, string description)
        {
            ParentId = parentId;
            ProductId = productId;
            Title = title;
            Description = description;
        }

        public static ProductDocumentation CreateInstance(long parentId, long productId,
            string title, string description)
        {
            ProductDocumentation productDocumentation = new(parentId, productId, title, description);
            return productDocumentation;
        }
    }
}
