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
        public long ProductId { get; set; }
        public AddSprintCommand(long productId, string name, DateTime startDate, DateTime endDate)
        {
            ProductId = productId;
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
        }

        internal sealed class AddSprintCommandHandler : ICommandHandler<AddSprintCommand>
        {
            private readonly ISprintRepository _sprintRepository;
            private readonly IProductRepository _productRepository;
            private readonly IUnitOfWork _unitOfWork;

            public AddSprintCommandHandler(
                ISprintRepository sprintRepository,
                IProductRepository productRepository,
                IUnitOfWork unitOfWork)
            {
                _sprintRepository = sprintRepository;
                _productRepository = productRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result> Handle(AddSprintCommand command)
            {
                Sprint sprintWithSameName = _sprintRepository.GetByName(command.Name);

                if (sprintWithSameName != null)
                    return Result.Failure($"Sprint '{command.Name}' already exists");

                Product product = await _productRepository.GetById(command.ProductId);

                if (product == null)
                    return Result.Failure("Invalid product id");

                var sprintResult = Sprint.CreateInstance(product, command.Name, command.StartDate, command.EndDate);
                
                if (sprintResult.IsFailure)
                    return Result.Failure(sprintResult.Error);

                _sprintRepository.AddSprint(sprintResult.Value);
                await _unitOfWork.CompleteAsync();


                return Result.Success();
            }
        }
    }
}
