using CSharpFunctionalExtensions;
using ProductFocus.Domain.Common;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Threading.Tasks;
using MediatR;
using System.Threading;

namespace ProductFocusApi.CommandHandlers
{
    public sealed class AddTagCategoryCommand : IRequest<Result>
    {
        public long ProductId { get; }
        public string Name { get; }
        public AddTagCategoryCommand(long productId, string name)
        {
            ProductId = productId;
            Name = name;
        }
        internal sealed class AddTagCategoryCommandHandler : IRequestHandler<AddTagCategoryCommand, Result>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ITagCategoryRepository _tagCategoryRepository;
            private readonly IProductRepository _productRepository;
            public AddTagCategoryCommandHandler(
                IUnitOfWork unitOfWork,
                ITagCategoryRepository tagCategoryRepository,
                IProductRepository produtRepository)
            {
                _unitOfWork = unitOfWork;
                _tagCategoryRepository = tagCategoryRepository;
                _productRepository = produtRepository;
            }
            public async Task<Result> Handle(AddTagCategoryCommand request, CancellationToken cancellationToken)
            {
                Product product = await _productRepository.GetById(request.ProductId);
                if(product == null)
                {
                    return Result.Failure<Result>($"No Product found with product id '{request.ProductId}'");
                }

                try
                {
                    TagCategory tagCategory = TagCategory.CreateInstance(product, request.Name);
                    _tagCategoryRepository.AddTagCategory(tagCategory);
                    await _unitOfWork.CompleteAsync(cancellationToken);
                    return Result.Success();
                }
                catch(Exception ex)
                {
                    return Result.Failure(ex.Message);
                }
            }
        }
    }
}
