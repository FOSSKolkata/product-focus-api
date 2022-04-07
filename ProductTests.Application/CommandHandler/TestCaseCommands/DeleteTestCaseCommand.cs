using CSharpFunctionalExtensions;
using MediatR;
using ProductTests.Domain.Common;
using ProductTests.Domain.Model.TestCaseAggregate;
using ProductTests.Domain.Model.TestPlanAggregate;
using ProductTests.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProductTests.Application.CommandHandler.TestCaseCommands
{
    public sealed class DeleteTestCaseCommand : IRequest<Result>
    {
        public long Id { get; private set; }
        public string UserId { get; private set; }
        public long? TestPlanId { get; private set; }
        public long? TestSuiteId { get; private set; }
        public DeleteTestCaseCommand(long id, long? testPlanId, long? testSuiteId, string userId)
        {
            Id = id;
            UserId = userId;
            TestPlanId = testPlanId;
            TestSuiteId = testSuiteId;
        }
        internal sealed class DeleteTestCaseCommandHandler : IRequestHandler<DeleteTestCaseCommand, Result>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ITestCaseRepository _testCaseRepository;
            private readonly ITestPlanRepository _testPlanRepository;
            public DeleteTestCaseCommandHandler(IUnitOfWork unitOfWork, ITestCaseRepository testCaseRepository, ITestPlanRepository testPlanRepository)
            {
                _unitOfWork = unitOfWork;
                _testCaseRepository = testCaseRepository;
                _testPlanRepository = testPlanRepository;
            }
            public async Task<Result> Handle(DeleteTestCaseCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    if (request.TestPlanId is not null && request.TestSuiteId is not null)
                    {
                        TestPlan testPlan = await _testPlanRepository.GetById(request.TestPlanId??0);
                        testPlan.DeleteTestSuiteTestCaseMapping(request.TestSuiteId??0, request.Id, request.UserId);
                    }
                    TestCase testCase = await _testCaseRepository.GetById(request.Id);
                    testCase.Delete(request.UserId);
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
