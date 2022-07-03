using CSharpFunctionalExtensions;
using MediatR;
using ProductFocus.Domain.Common;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductFocusApi.CommandHandlers
{
    public sealed class DeleteSprintCommand: IRequest<Result>
    {
        public long Id { get; private set; }
        public string UserObjectId { get; private set; }
        public DeleteSprintCommand(long id, string userObjectId)
        {
            Id = id;
            UserObjectId = userObjectId;
        }
        internal class DeleteSprintCommandHandler : IRequestHandler<DeleteSprintCommand, Result>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ISprintRepository _sprintRepository;
            public DeleteSprintCommandHandler(IUnitOfWork unitOfWork, ISprintRepository sprintRepository)
            {
                _unitOfWork = unitOfWork;
                _sprintRepository = sprintRepository;
            }

            public async Task<Result> Handle(DeleteSprintCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    Sprint sprint = await _sprintRepository.GetById(request.Id);
                    if (sprint == null)
                        return Result.Failure($"No sprint exist with id {request.Id} to delete");
                    sprint.Delete(request.UserObjectId);
                    await _unitOfWork.CompleteAsync(cancellationToken);
                    return Result.Success();
                }catch(Exception)
                {
                    throw;
                }
            }
        }
    }
}
