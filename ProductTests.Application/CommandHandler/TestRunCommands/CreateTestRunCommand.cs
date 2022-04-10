using CSharpFunctionalExtensions;
using MediatR;
using ProductTests.Domain.Common;
using ProductTests.Domain.Model.TestCaseVersionAggregate;
using ProductTests.Domain.Model.TestPlanAggregate;
using ProductTests.Domain.Model.TestPlanVersionAggregate;
using ProductTests.Domain.Repositories;
using System;
using System.Collections.Generic;
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
            private readonly ITestPlanVersionRepository _testPlanVersionRepository;
            public CreateTestRunCommandHandler(ITestPlanRepository testPlanRepository,
                ITestCaseVersionRepository testCaseVersionRepository,
                ITestPlanVersionRepository testPlanVersionRepository,
                IUnitOfWork unitOfWork)
            {
                _testPlanRepository = testPlanRepository;
                _testCaseVersionRepository = testCaseVersionRepository;
                _testPlanVersionRepository = testPlanVersionRepository;
                _unitOfWork = unitOfWork;
            }
            public async Task<Result> Handle(CreateTestRunCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    TestPlan testPlan = await _testPlanRepository.GetById(request.TestPlanId);
                    TestPlanVersion testPlanVersion = TestPlanVersion.CreateInstance(testPlan);
                    Dictionary<long, TestCaseVersion> testCaseMap = new(); // NOTE: Store testCaseId and testCaseVersion
                    List<KeyValuePair<TestSuiteVersion, TestCaseVersion>> list = new();
                    foreach(TestSuite testSuite in testPlan.TestSuites)
                    {
                        TestSuiteVersion testSuiteVersion = TestSuiteVersion.CreateInstance(testSuite, testPlan.Id);
                        testPlanVersion.AddTestSuiteVersion(testSuiteVersion);
                        foreach (TestSuiteTestCaseMapping mapping in testSuite.TestSuiteTestCaseMappings)
                        {
                            if(!testCaseMap.ContainsKey(mapping.TestCase.Id))
                            {
                                TestCaseVersion testCaseVersion = TestCaseVersion.CreateInstance(mapping.TestCase);
                                list.Add(new(testSuiteVersion, testCaseVersion));
                                testCaseMap.Add(mapping.TestCase.Id, testCaseVersion);
                                _testCaseVersionRepository.Add(testCaseVersion);
                            }
                            else
                            {
                                TestCaseVersion testCaseVersion = testCaseMap.GetValueOrDefault(mapping.TestCase.Id);
                                list.Add(new(testSuiteVersion, testCaseVersion));
                            }
                        }
                    }
                    foreach(KeyValuePair<TestSuiteVersion,TestCaseVersion> item in list)
                    {
                        // Key -> TestSuiteVersion, Value -> TestCaseVersion
                        item.Key.AddTestSuiteTestCaseMappingVersion(item.Value);
                    }
                    _testPlanVersionRepository.Add(testPlanVersion);
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
