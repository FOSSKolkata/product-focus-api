using CSharpFunctionalExtensions;
using MediatR;
using ProductDocumentations.Domain.Common;
using ProductDocumentations.Domain.Model;
using ProductDocumentations.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductDocumentations.Application.CommandHandlers.UpdateProductDocumentation
{
    public sealed class UpdateProductDocumentationCommand : IRequest<Result<int>>
    {
        public long Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public UpdateDocumentationFieldName FieldName { get; private set; }
        
        public enum UpdateDocumentationFieldName
        {
            Title = 1,
            Description = 2
        }
        public UpdateProductDocumentationCommand(long id, string title, string description, UpdateDocumentationFieldName fieldName)
        {
            Id = id;
            Title = title;
            Description = description;
            FieldName = fieldName;
        }
        internal sealed class UpdateProductcommandHandler : IRequestHandler<UpdateProductDocumentationCommand, Result<int>>
        {
            private readonly IProductDocumentationRepository _productDocumentationRepository;
            private readonly IUnitOfWork _unitOfWork;
            public UpdateProductcommandHandler(IProductDocumentationRepository productDocumentationRepository, IUnitOfWork unitOfWork)
            {
                _productDocumentationRepository = productDocumentationRepository;
                _unitOfWork = unitOfWork;
            }
            public async Task<Result<int>> Handle(UpdateProductDocumentationCommand request, CancellationToken cancellationToken)
            {
                ProductDocumentation productDocumentation = await _productDocumentationRepository.GetById(request.Id);
                if (productDocumentation == null)
                    return Result.Failure<int>($"No product documentation found with id {request.Id}");
                try
                {
                    if(request.FieldName == UpdateDocumentationFieldName.Title)
                    {
                        productDocumentation.UpdateTitle(request.Title);
                    }
                    
                    if(request.FieldName == UpdateDocumentationFieldName.Description)
                    {
                        productDocumentation.UpdateDescription(request.Description);
                    }

                    await _unitOfWork.CompleteAsync(cancellationToken);
                    return Result.Success(1);
                }
                catch(Exception ex)
                {
                    return Result.Failure<int>(ex.Message);
                }
            }
        }
    }
}
