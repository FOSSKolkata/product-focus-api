using CSharpFunctionalExtensions;
using MediatR;
using ProductDocumentations.Domain.Common;
using ProductDocumentations.Domain.Model;
using ProductDocumentations.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductDocumentations.Application.CommandHandlers.AddProductDocumentation
{
    public sealed class AddProductDocumentationCommand : IRequest<Result>
    {
        public long? ParentId { get; private set; }
        public long ProductId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }

        public AddProductDocumentationCommand(long? parentId, long productId, string title, string description)
        {
            ParentId = parentId;
            ProductId = productId;
            Title = title;
            Description = description;
        }

        public sealed class AddProductDocumentationCommandHandler : IRequestHandler<AddProductDocumentationCommand,Result>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IProductDocumentationRepository _productDocumentationRepository;
            public AddProductDocumentationCommandHandler(IUnitOfWork unitOfWork, IProductDocumentationRepository productDocumentationRepository)
            {
                _unitOfWork = unitOfWork;
                _productDocumentationRepository = productDocumentationRepository;
            }

            public async Task<Result> Handle(AddProductDocumentationCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    ProductDocumentation productDocumentation = ProductDocumentation.CreateInstance(request.ParentId, request.ProductId, request.Title, request.Description);
                    _productDocumentationRepository.Add(productDocumentation);
                    await _unitOfWork.CompleteAsync(cancellationToken);
                    return Result.Success(productDocumentation);
                }
                catch(Exception ex)
                {
                    return Result.Failure(ex.Message);
                }
            }
        }
    }
}
