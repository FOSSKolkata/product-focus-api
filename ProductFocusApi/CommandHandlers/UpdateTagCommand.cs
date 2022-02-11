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
    public class UpdateTagCommand : IRequest<Result>
    {
        public long TagId { get; private set; }
        public string UserId { get; private set; }
        public UpdateTagCommand(long tagId, string userId)
        {
            TagId = tagId;
            UserId = userId;
        }
        internal sealed class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommand, Result>
        {
            private readonly ITagRepository _tagRepository;
            private readonly IUnitOfWork _unitOfWork;
            public UpdateTagCommandHandler(ITagRepository tagRepository, IUnitOfWork unitOfWork)
            {
                _tagRepository = tagRepository;
                _unitOfWork = unitOfWork;
            }
            public async Task<Result> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
            {
                Tag tag = await _tagRepository.GetById(request.TagId);
                if(tag == null)
                {
                    return Result.Failure<Tag>($"Invalid tag with id {request.TagId}");
                }

                try
                {
                    tag.Delete(request.UserId);
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
