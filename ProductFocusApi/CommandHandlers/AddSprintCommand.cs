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
    public class AddSprintCommand : IRequest<Result>
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

        internal sealed class AddSprintCommandHandler : IRequestHandler<AddSprintCommand, Result>
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

            public async Task<Result> Handle(AddSprintCommand request, CancellationToken cancellationToken)
            {
                Sprint sprintWithSameName = _sprintRepository.GetByName(request.Name);

                if (sprintWithSameName != null)
                    return Result.Failure($"Sprint '{request.Name}' already exists");

                Product product = await _productRepository.GetById(request.ProductId);

                if (product == null)
                    return Result.Failure("Invalid product id");

                var sprintResult = Sprint.CreateInstance(product, request.Name, request.StartDate, request.EndDate);
                
                if (sprintResult.IsFailure)
                    return Result.Failure(sprintResult.Error);

                _sprintRepository.AddSprint(sprintResult.Value);
                await _unitOfWork.CompleteAsync(cancellationToken);


                return Result.Success();
            }
        }
    }
}
