using CSharpFunctionalExtensions;
using MediatR;
using ProductTests.Domain.Common;
using ProductTests.Domain.Model.TestPlanAggregate;
using ProductTests.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductTests.Application.CommandHandler.TestSuiteCommands
{
    public sealed class DeleteTestSuiteCommand : IRequest<Result>
    {
        public long Id { get; private set; }
        public long SuiteId { get; private set; }
        public string UserId { get; private set; }
        public DeleteTestSuiteCommand(long id, long suiteId, string userId)
        {
            Id = id;
            SuiteId = suiteId;
            UserId = userId;
        }
        internal sealed class DeleteTestSuiteCommandHandler : IRequestHandler<DeleteTestSuiteCommand, Result>
        {
            private readonly ITestPlanRepository _testPlanRepository;
            private readonly IUnitOfWork _unitOfWork;
            public DeleteTestSuiteCommandHandler(ITestPlanRepository testPlanRepository, IUnitOfWork unitOfWork)
            {
                _testPlanRepository = testPlanRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result> Handle(DeleteTestSuiteCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    TestPlan testPlan = await _testPlanRepository.GetById(request.Id);
                    testPlan.DeleteTestSuite(request.SuiteId, request.UserId);
                    await _unitOfWork.CompleteAsync(cancellationToken);
                    return Result.Success();
                }
                catch(Exception ex)
                {
                    return Result.Failure(ex.Message);
                }
            }
        }
    }
}
