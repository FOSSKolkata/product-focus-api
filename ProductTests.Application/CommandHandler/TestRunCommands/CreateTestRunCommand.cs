using CSharpFunctionalExtensions;
using MediatR;
using ProductTests.Domain.Common;
using ProductTests.Domain.Model.TestCaseVersionAggregate;
using ProductTests.Domain.Model.TestPlanAggregate;
using ProductTests.Domain.Model.TestPlanVersionAggregate;
using ProductTests.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductTests.Application.CommandHandler.TestRunCommands
{
    public class CreateTestRunCommand : IRequest<Result>
    {
        public long TestPlanId { get; private set; }
        public CreateTestRunCommand(long testPlanId)
        {
            TestPlanId = testPlanId;
        }
        internal class CreateTestRunCommandHandler : IRequestHandler<CreateTestRunCommand, Result>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ITestPlanRepository _testPlanRepository;
            private readonly ITestCaseVersionRepository _testCaseVersionRepository;
            public CreateTestRunCommandHandler(ITestPlanRepository testPlanRepository,
                ITestCaseVersionRepository testCaseVersionRepository,
                IUnitOfWork unitOfWork)
            {
                _testPlanRepository = testPlanRepository;
                _testCaseVersionRepository = testCaseVersionRepository;
                _unitOfWork = unitOfWork;
            }
            public async Task<Result> Handle(CreateTestRunCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    TestPlan testPlan = await _testPlanRepository.GetById(request.TestPlanId);
                    TestPlanVersion testPlanVersion = TestPlanVersion.CreateInstance(testPlan);
                    foreach(TestSuite testSuite in testPlan.TestSuites)
                    {
                        testPlanVersion.AddTestSuiteVersion(testSuite);
                        foreach(TestSuiteTestCaseMapping mapping in testSuite.TestSuiteTestCaseMappings)
                        {
                            _testCaseVersionRepository.Add(TestCaseVersion.CreateInstance(mapping.TestCase));
                        }
                    }
                    await _unitOfWork.CompleteAsync();
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
