using CSharpFunctionalExtensions;
using ProductFocus.Domain;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace ProductFocusApi.CommandHandlers
{
    public class UpdateTagCommand : ICommand
    {
        public long TagId { get; private set; }
        public string UserId { get; private set; }
        public UpdateTagCommand(long tagId, string userId)
        {
            TagId = tagId;
            UserId = userId;
        }
        internal sealed class UpdateTagCommandHandler : ICommandHandler<UpdateTagCommand>
        {
            private readonly ITagRepository _tagRepository;
            private readonly IUnitOfWork _unitOfWork;
            public UpdateTagCommandHandler(ITagRepository tagRepository, IUnitOfWork unitOfWork)
            {
                _tagRepository = tagRepository;
                _unitOfWork = unitOfWork;
            }
            public async Task<Result> Handle(UpdateTagCommand command)
            {
                Tag tag = await _tagRepository.GetById(command.TagId);
                if(tag == null)
                {
                    return Result.Failure<Tag>($"Invalid tag with id {command.TagId}");
                }

                try
                {
                    tag.Delete(command.UserId);
                    await _unitOfWork.CompleteAsync();
                }
                catch(Exception ex)
                {
                    return Result.Failure<Tag>(ex.Message);
                }
                return Result.Success();
            }
        }
    }
}
