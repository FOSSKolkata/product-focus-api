using CSharpFunctionalExtensions;
using MediatR;
using ProductTests.Domain.Common;
using ProductTests.Domain.Model.TestCaseAggregate;
using ProductTests.Domain.Model.TestPlanAggregate;
using ProductTests.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProductTests.Application.CommandHandler.TestCaseCommands
{
    public sealed class AddTestCaseCommand : IRequest<Result>
    {
        public string Title { get; private set; }
        public string Preconditions { get; private set; }
        public long? TestPlanId { get; private set; }
        public long? TestSuiteId { get; private set; }
        public List<AddTestStepDto> TestSteps { get; private set; }
        public AddTestCaseCommand(string title, string preconditins, long? testPlanId, long? testSuiteId, List<AddTestStepDto> testSteps)
        {
            Title = title;
            Preconditions = preconditins;
            TestPlanId = testPlanId;
            TestSuiteId = testSuiteId;
            TestSteps = testSteps;
        }
        internal sealed class AddTestCaseCommandHandler : IRequestHandler<AddTestCaseCommand, Result>
        {
            private readonly ITestCaseRepository _testCaseRepository;
            private readonly ITestPlanRepository _testPlanRepository;
            private readonly IUnitOfWork _unitOfWork;
            public AddTestCaseCommandHandler(ITestCaseRepository testCaseRepository, ITestPlanRepository testPlanRepository, IUnitOfWork unitOfWork)
            {
                _testPlanRepository = testPlanRepository;
                _testCaseRepository = testCaseRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result> Handle(AddTestCaseCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    List<TestStep> testSteps = new();
                    for (int i = 0; i < request.TestSteps.Count; i++)
                    {
                        AddTestStepDto testStep = request.TestSteps[i];
                        testSteps.Add(testStep.ToTestStep(i + 1));
                    }

                    TestCase testCase = TestCase.CreateInstance(request.Title, request.Preconditions, testSteps);
                    if(request.TestPlanId is not null && request.TestSuiteId is not null)
                    {
                        TestPlan testPlan = await _testPlanRepository.GetById(request.TestPlanId??0);
                        testPlan.AddTestSuiteTestCaseMapping(request.TestSuiteId ?? 0, testCase);
                    }
                    
                    _testCaseRepository.Add(testCase);

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
