using CSharpFunctionalExtensions;
using MediatR;
using ProductDocumentations.Domain.Common;
using ProductDocumentations.Domain.Model;
using ProductDocumentations.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductDocumentations.Application.CommandHandlers.DeleteProductDocumentation
{
    public class DeleteProductDocumentationCommand : IRequest<Result>
    {
        public virtual long Id { get; private set; }
        public virtual string UserId { get; private set; }

        public DeleteProductDocumentationCommand(long id, string userId)
        {
            Id = id;
            UserId = userId;
        }
        internal sealed class DeleteProductDocumentationCommandHandler : IRequestHandler<DeleteProductDocumentationCommand, Result>
        {
            private readonly IProductDocumentationRepository _productDocumentationRepository;
            private readonly IUnitOfWork _unitOfWork;

            public DeleteProductDocumentationCommandHandler(IProductDocumentationRepository productDocumentationRepository, IUnitOfWork unitOfWork)
            {
                _productDocumentationRepository = productDocumentationRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result> Handle(DeleteProductDocumentationCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    ProductDocumentation productDocumentation = await _productDocumentationRepository.GetById(request.Id);
                    productDocumentation.Delete(request.UserId);
                    await _unitOfWork.CompleteAsync(cancellationToken);
                    return Result.Success();
                }
                catch(Exception ex)
                {
                    return Result.Failure<Result>(ex.Message);
                }
            }
        }
    }
}
