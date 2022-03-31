using CSharpFunctionalExtensions;
using MediatR;
using ProductTests.Domain.Common;
using ProductTests.Domain.Model.TestPlanAggregate;
using ProductTests.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductTests.Application.CommandHandler.TestPlanCommands
{
    public sealed class DeleteTestPlanCommand : IRequest<Result>
    {
        public long Id { get; private set; }
        public string UserId { get; private set; }
        public DeleteTestPlanCommand(long id, string userId)
        {
            Id = id;
            UserId = userId;
        }
        internal sealed class DeleteTestPlanCommandHandler : IRequestHandler<DeleteTestPlanCommand, Result>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ITestPlanRepository _testPlanRepository;

            public DeleteTestPlanCommandHandler(IUnitOfWork unitOfWork, ITestPlanRepository testPlanRepository)
            {
                _unitOfWork = unitOfWork;
                _testPlanRepository = testPlanRepository;
            }
            public async Task<Result> Handle(DeleteTestPlanCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    TestPlan testPlan = await _testPlanRepository.GetById(request.Id);
                    testPlan.Delete(request.UserId);
                    await _unitOfWork.CompleteAsync(cancellationToken);
                    return Result.Success(testPlan);
                }
                catch(Exception ex)
                {
                    return Result.Failure(ex.Message);
                }
            }
        }
    }
}
