using CSharpFunctionalExtensions;
using ProductFocus.Domain.Common;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace ProductFocusApi.CommandHandlers
{
    public sealed class AddTagCommand : ICommand
    {
        public long ProductId { get; }
        public string TagName { get; }
        public long? TagCategoryId { get; }
        public AddTagCommand(long productId, string tagName, long? tagCategoryId)
        {
            ProductId = productId;
            TagName = tagName;
            TagCategoryId = tagCategoryId;
        }
        internal sealed class AddTagCommandHandler : ICommandHandler<AddTagCommand>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ITagRepository _tagRepository;
            private readonly ITagCategoryRepository _tagCategoryRepository;
            public AddTagCommandHandler(IUnitOfWork unitOfWork,
                ITagRepository tagRepository,
                ITagCategoryRepository tagCategoryRepository)
            {
                _unitOfWork = unitOfWork;
                _tagRepository = tagRepository;
                _tagCategoryRepository = tagCategoryRepository;
            }

            public async Task<Result> Handle(AddTagCommand command)
            {

                try
                {
                    TagCategory tagCategory = command.TagCategoryId == null ? null : await _tagCategoryRepository.GetById((long)command.TagCategoryId);
                    Tag alreadyPresentTagInProduct = await _tagRepository.GetByNameAndProductId(command.TagName, command.ProductId);
                    if(alreadyPresentTagInProduct == null)
                    {

                        Tag tag = Tag.CreateInstance(command.TagName, command.ProductId, tagCategory).Value;
                        _tagRepository.AddTag(tag);
                    }
                    else if(alreadyPresentTagInProduct.IsDeleted == true)
                    {
                        alreadyPresentTagInProduct.IsDeleted = false;
                    }
                    else
                    {
                        return Result.Failure($"Tag is already present with the name {command.TagName}");
                    }
                    await _unitOfWork.CompleteAsync();
                    return Result.Success();
                } catch(Exception ex)
                {
                    return Result.Failure(ex.Message);
                }
            }
        }
    }
}
