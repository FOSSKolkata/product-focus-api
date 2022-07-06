using CSharpFunctionalExtensions;
using MediatR;
using ProductFocus.Domain.Common;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProductFocusApi.CommandHandlers
{
    public class UpdateSprintCommand : IRequest<Result>
    {
        public long Id { get; private set; }
        public long ProductId { get; private set; }
        public string Name { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public UpdateSprintCommand(long id, long productId, string name, DateTime startDate, DateTime endDate)
        {
            Id = id;
            ProductId = productId;
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
        }
        internal sealed class UpdateSprintCommandHandler : IRequestHandler<UpdateSprintCommand, Result>
        {
            private readonly ISprintRepository _sprintRepository;
            private readonly IUnitOfWork _unitOfWork;
            public UpdateSprintCommandHandler(ISprintRepository sprintRepository, IUnitOfWork unitOfWork)
            {
                _sprintRepository = sprintRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result> Handle(UpdateSprintCommand request, CancellationToken cancellationToken)
            {
                List<Sprint> sprints = await _sprintRepository.GetByProductId(request.ProductId);
                Sprint sprintNeedToUpdate = await _sprintRepository.GetById(request.Id);
                if (sprintNeedToUpdate == null)
                    return Result.Failure($"Sprint doesn't exist");
                foreach(var sprint in sprints)
                {
                    if (sprint.Id == sprintNeedToUpdate.Id)
                        continue;

                    if (sprint.Name == request.Name)
                        return Result.Failure($"sprint with name '{request.Name}' already exist");

                    if (sprint.EndDate < request.StartDate || sprint.StartDate > request.EndDate)
                        continue;
                }
                sprintNeedToUpdate.Update(request.Name, request.StartDate, request.EndDate);
                await _unitOfWork.CompleteAsync(cancellationToken);
                return Result.Success();
            }
        }
    }
}
