using CSharpFunctionalExtensions;
using ProductFocus.Domain.Common;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace ProductFocusApi.CommandHandlers
{
    public sealed class AddTagCategoryCommand : ICommand
    {
        public long ProductId { get; }
        public string Name { get; }
        public AddTagCategoryCommand(long productId, string name)
        {
            ProductId = productId;
            Name = name;
        }
        internal sealed class AddTagCategoryCommandHandler : ICommandHandler<AddTagCategoryCommand>
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
            public async Task<Result> Handle(AddTagCategoryCommand command)
            {
                Product product = await _productRepository.GetById(command.ProductId);
                if(product == null)
                {
                    return Result.Failure<Result>($"No Product found with product id '{command.ProductId}'");
                }

                try
                {
                    TagCategory tagCategory = TagCategory.CreateInstance(product, command.Name);
                    _tagCategoryRepository.AddTagCategory(tagCategory);
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
}
