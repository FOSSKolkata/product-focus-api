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
    public sealed class AddTagCommand : IRequest<Result>
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
        internal sealed class AddTagCommandHandler : IRequestHandler<AddTagCommand, Result>
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

            public async Task<Result> Handle(AddTagCommand request, CancellationToken cancellationToken)
            {

                try
                {
                    TagCategory tagCategory = request.TagCategoryId == null ? null : await _tagCategoryRepository.GetById((long)request.TagCategoryId);
                    Tag alreadyPresentTagInProduct = await _tagRepository.GetByNameAndProductId(request.TagName, request.ProductId);
                    if(alreadyPresentTagInProduct == null)
                    {

                        Tag tag = Tag.CreateInstance(request.TagName, request.ProductId, tagCategory).Value;
                        _tagRepository.AddTag(tag);
                    }
                    else if(alreadyPresentTagInProduct.IsDeleted == true)
                    {
                        alreadyPresentTagInProduct.IsDeleted = false;
                    }
                    else
                    {
                        return Result.Failure($"Tag is already present with the name {request.TagName}");
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
