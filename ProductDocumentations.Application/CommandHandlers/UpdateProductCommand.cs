using CSharpFunctionalExtensions;
using MediatR;
using ProductDocumentations.Domain.Common;
using ProductDocumentations.Domain.Model;
using ProductDocumentations.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductDocumentations.Application.CommandHandlers
{
    public sealed class UpdateProductCommand : IRequest<Result<ProductDocumentationDto>>
    {
        public UpdateDocumentationDto UpdateDocumentationDto { get; }
        public UpdateProductCommand(UpdateDocumentationDto updateDocumentationDto)
        {
            UpdateDocumentationDto = updateDocumentationDto;
        }
        internal sealed class UpdateProductcommandHandler : IRequestHandler<UpdateProductCommand, Result<ProductDocumentationDto>>
        {
            private readonly IProductDocumentationRepository _productDocumentationRepository;
            private readonly IUnitOfWork _unitOfWork;
            public UpdateProductcommandHandler(IProductDocumentationRepository productDocumentationRepository, IUnitOfWork unitOfWork)
            {
                _productDocumentationRepository = productDocumentationRepository;
                _unitOfWork = unitOfWork;
            }
            public async Task<Result<ProductDocumentationDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
            {
                ProductDocumentation productDocumentation = await _productDocumentationRepository.GetById(request.UpdateDocumentationDto.Id);
                if (productDocumentation == null)
                    return Result.Failure<ProductDocumentationDto>($"No product documentation found with id {request.UpdateDocumentationDto.Id}");
                try
                {
                    if(request.UpdateDocumentationDto.FieldName == UpdateDocumentationFieldName.Title)
                    {
                        productDocumentation.UpdateTitle(request.UpdateDocumentationDto.Title);
                    }
                    
                    if(request.UpdateDocumentationDto.FieldName == UpdateDocumentationFieldName.Description)
                    {
                        productDocumentation.UpdateDescription(request.UpdateDocumentationDto.Description);
                    }

                    await _unitOfWork.CompleteAsync(cancellationToken);
                    return Result.Success(new ProductDocumentationDto(productDocumentation.ParentId,
                        productDocumentation.ProductId,
                        productDocumentation.Title,
                        productDocumentation.Description));
                }
                catch(Exception ex)
                {
                    return Result.Failure<ProductDocumentationDto>(ex.Message);
                }
            }
        }
    }
}
