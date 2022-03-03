using CSharpFunctionalExtensions;
using MediatR;
using ProductDocumentations.Domain.Common;
using ProductDocumentations.Domain.Model;
using ProductDocumentations.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProductDocumentations.Application.CommandHandlers.UpdateProductDocumentation
{
    public sealed class UpdateProductDocumentationOrderingCommand : IRequest<Result>
    {
        public List<OrderingInfo> OrderingInfos { get; set; }

        internal sealed class UpdateProductDocumentationCommandHandler : IRequestHandler<UpdateProductDocumentationOrderingCommand, Result>
        {
            private readonly IProductDocumentationRepository _productDocumentationRepository;
            private readonly IUnitOfWork _unitOfWork;
            public UpdateProductDocumentationCommandHandler(IProductDocumentationRepository productDocumentationRepository, IUnitOfWork unitOfWork)
            {
                _productDocumentationRepository = productDocumentationRepository;
                _unitOfWork = unitOfWork;
            }
            public async Task<Result> Handle(UpdateProductDocumentationOrderingCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    foreach (OrderingInfo orderingInfo in request.OrderingInfos)
                    {
                        ProductDocumentation productDocumentation = await _productDocumentationRepository.GetById(orderingInfo.Id);
                        productDocumentation.UpdateOrderingNumber(orderingInfo.OrderNumber);
                    }
                    await _unitOfWork.CompleteAsync();
                    return Result.Success();
                }
                catch(Exception ex)
                {
                    return Result.Failure(ex.Message);
                }
            }
        }
    }
    public sealed class OrderingInfo
    {
        public long Id { get; set; }
        public long OrderNumber { get; set; }
    }
}
