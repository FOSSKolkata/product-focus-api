using ProductDocumentations.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductDocumentations.Domain.Model
{
    [Table("ProductDocumentationAttachments")]
    public class ProductDocumentationAttachment : AggregateRoot<long>
    {
        public virtual long ProductDocumentationId { get; private set; }
        public virtual string Name { get; private set; }
        public virtual string Uri { get; private set; }
        public virtual string FileName { get; private set; }
        protected ProductDocumentationAttachment()
        {

        }
        private ProductDocumentationAttachment(long productDocumentationId, string name, string uri, string fileName)
        {
            ProductDocumentationId = productDocumentationId;
            Name = name;
            Uri = uri;
            FileName = fileName;
        }

        public static ProductDocumentationAttachment CreateInstance(long productDocumentationId, string name, string uri, string fileName)
        {
            ProductDocumentationAttachment productDocumentationAttachment = new(productDocumentationId, name, uri, fileName);
            return productDocumentationAttachment;
        }
    }
}
