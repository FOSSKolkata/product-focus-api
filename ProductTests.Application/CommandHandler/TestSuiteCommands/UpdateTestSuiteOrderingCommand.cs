using CSharpFunctionalExtensions;
using MediatR;
using ProductTests.Domain.Common;
using ProductTests.Domain.Model.TestPlanAggregate;
using ProductTests.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProductTests.Application.CommandHandler.TestSuiteCommands
{
    public sealed class UpdateTestSuiteOrderingCommand : IRequest<Result>
    {
        public long TestPlanId { get; private set; }
        public List<UpdateTestSuiteOrderingDto> TestSuiteIds { get; private set; }
        public UpdateTestSuiteOrderingCommand(long testPlanId, List<UpdateTestSuiteOrderingDto> testSuiteIds)
        {
            TestPlanId = testPlanId;
            TestSuiteIds = testSuiteIds;
        }
        internal class UpdateTestSuiteOrderingCommandHandler : IRequestHandler<UpdateTestSuiteOrderingCommand, Result>
        {
            private readonly ITestPlanRepository _testPlanRepository;
            private readonly IUnitOfWork _unitOfWork;
            public UpdateTestSuiteOrderingCommandHandler(ITestPlanRepository testPlanRepository, IUnitOfWork unitOfWork)
            {
                _testPlanRepository = testPlanRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result> Handle(UpdateTestSuiteOrderingCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    TestPlan testPlan = await _testPlanRepository.GetById(request.TestPlanId);
                    List<long> updatedOrderedIds = new();
                    foreach(var testSuite in request.TestSuiteIds)
                    {
                        updatedOrderedIds.Add(testSuite.Id);
                    }
                    testPlan.UpdateSuiteOrders(updatedOrderedIds);
                    await _unitOfWork.CompleteAsync(cancellationToken);

                }
                catch(Exception ex)
                {
                    return Result.Failure(ex.Message);
                }
                return Result.Success();
            }
        }
    }
}
