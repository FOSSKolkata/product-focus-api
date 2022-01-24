using ProductDocumentations.Domain.Model;
using ProductDocumentations.Domain.Repositories;
using ProductFocus.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductDocumentations.Infrastructure.Repositories
{
    public class ProductDocumentationRepository : IProductDocumentationRepository
    {
        private readonly UnitOfWork _unitOfWork;
        public void Add(ProductDocumentation productDocumentation)
        {
            throw new NotImplementedException();
        }
    }
}
