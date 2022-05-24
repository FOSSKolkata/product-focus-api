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
    public sealed class AddTestSuiteCommand : IRequest<Result>
    {
        public string Name { get; private set; }
        public long TestPlanId { get; private set; }
        public AddTestSuiteCommand(string name, long testPlanId)
        {
            Name = name;
            TestPlanId = testPlanId;
        }
        internal sealed class AddTestSuiteCommandHandler : IRequestHandler<AddTestSuiteCommand, Result>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ITestPlanRepository _testPlanRepository;
            public AddTestSuiteCommandHandler(IUnitOfWork unitOfWork, ITestPlanRepository testPlanRepository)
            {
                _unitOfWork = unitOfWork;
                _testPlanRepository = testPlanRepository;
            }
            public async Task<Result> Handle(AddTestSuiteCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    TestPlan testPlan = await _testPlanRepository.GetById(request.TestPlanId);
                    foreach(var testSuite in testPlan.TestSuites)
                    {
                        if(testSuite.Name == request.Name)
                        {
                            return Result.Failure($"The title {request.Name} is already exist!");
                        }
                    }
                    testPlan.AddTestSuite(request.Name, request.TestPlanId);
                    await _unitOfWork.CompleteAsync(cancellationToken);
                    return Result.Success();
                }
                catch (Exception ex)
                {
                    return Result.Failure(ex.Message);
                }
            }
        }
    }
}
