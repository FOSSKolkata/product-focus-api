using CSharpFunctionalExtensions;
using ProductFocus.Domain;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using ProductFocusApi.Dtos;
using System;
using System.Threading.Tasks;

namespace ProductFocusApi.CommandHandlers
{
    public sealed class AddTagCommand : ICommand
    {
        public long ProductId { get; }
        public AddTagDto AddTagDto { get; }
        public AddTagCommand(long productId, AddTagDto addTagDto)
        {
            ProductId = productId;
            AddTagDto = addTagDto;
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
                    TagCategory tagCategory = command.AddTagDto.TagCategoryId == null ? null : await _tagCategoryRepository.GetById((long)command.AddTagDto.TagCategoryId);
                    Tag tag = Tag.CreateInstance(command.AddTagDto.Name, command.ProductId, tagCategory).Value;
                    _tagRepository.AddTag(tag);
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
