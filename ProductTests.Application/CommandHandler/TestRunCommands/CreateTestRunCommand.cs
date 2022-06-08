using CSharpFunctionalExtensions;
using MediatR;
using ProductTests.Domain.Common;
using ProductTests.Domain.Model.TestPlanAggregate;
using ProductTests.Domain.Model.TestRunAggregate;
using ProductTests.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductTests.Application.CommandHandler.TestRunCommands
{
    public class CreateTestRunCommand : IRequest<Result<long>>
    {
        public long TestPlanId { get; private set; }
        public CreateTestRunCommand(long testPlanId)
        {
            TestPlanId = testPlanId;
        }
        internal class CreateTestRunCommandHandler : IRequestHandler<CreateTestRunCommand, Result<long>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ITestPlanRepository _testPlanRepository;
            private readonly ITestRunRepository _testRunRepository;
            public CreateTestRunCommandHandler(ITestPlanRepository testPlanRepository,
                ITestRunRepository testRunRepository,
                IUnitOfWork unitOfWork)
            {
                _testPlanRepository = testPlanRepository;
                _testRunRepository = testRunRepository;
                _unitOfWork = unitOfWork;
            }
            public async Task<Result<long>> Handle(CreateTestRunCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    TestPlan testPlan = await _testPlanRepository.GetById(request.TestPlanId);
                    TestRun testRun = TestRun.CreateInstance(testPlan);
                    _testRunRepository.Add(testRun);
                    await _unitOfWork.CompleteAsync(cancellationToken);
                    return Result.Success(testRun.Id);
                }
                catch(Exception)
                {
                    throw;
                }
            }
        }
    }
}
