using CSharpFunctionalExtensions;
using ProductFocus.Domain;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductFocusApi.CommandHandlers
{
    public class AddSprintCommand : ICommand
    {
        public virtual string Name { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }

        public AddSprintCommand(string name, DateTime startDate, DateTime endDate)
        {
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
        }

        internal sealed class AddSprintCommandHandler : ICommandHandler<AddSprintCommand>
        {
            private readonly ISprintRepository _sprintRepository;
            private readonly IUnitOfWork _unitOfWork;

            public AddSprintCommandHandler(
                ISprintRepository sprintRepository,
                IUnitOfWork unitOfWork)
            {
                _sprintRepository = sprintRepository;
                _unitOfWork = unitOfWork;
            }
            public async Task<Result> Handle(AddSprintCommand command)
            {
                Sprint sprintWithSameName = _sprintRepository.GetByName(command.Name);


                if (sprintWithSameName != null)
                    return Result.Failure($"Sprint '{command.Name}' already exists");
                
                var sprintResult = Sprint.CreateInstance(command.Name, command.StartDate, command.EndDate);

                if (sprintResult.IsFailure)
                    return Result.Failure(sprintResult.Error);

                _sprintRepository.AddSprint(sprintResult.Value);
                await _unitOfWork.CompleteAsync();


                return Result.Success();
            }
        }
    }
}
