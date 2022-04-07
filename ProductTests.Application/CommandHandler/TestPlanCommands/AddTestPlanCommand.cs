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
    public sealed class AddTestPlanCommand : IRequest<Result>
    {
        public long ProductId { get; private set; }
        public long? SprintId { get; private set; }
        public TestTypeEnum TestType { get; private set; }
        public long ProductDocumentationId { get; private set; }
        public long? WorkItemId { get; private set; }
        public string Title { get; private set; }

        public AddTestPlanCommand(long productId, long? sprintId, string title, TestTypeEnum testType, long productDocumentation, long? workItemId)
        {
            ProductId = productId;
            SprintId = sprintId;
            Title = title;
            TestType = testType;
            ProductDocumentationId = productDocumentation;
            WorkItemId = workItemId;
        }
        internal sealed class AddTestPlanCommandHandler : IRequestHandler<AddTestPlanCommand, Result>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ITestPlanRepository _testPlanRepository;
            public AddTestPlanCommandHandler(IUnitOfWork unitOfWork, ITestPlanRepository testPlanRepository)
            {
                _unitOfWork = unitOfWork;
                _testPlanRepository = testPlanRepository;
            }
            public async Task<Result> Handle(AddTestPlanCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    TestPlan testPlan = TestPlan.CreateInstance(request.Title, request.ProductId, request.SprintId, request.TestType,
                        request.ProductDocumentationId, request.WorkItemId);
                    _testPlanRepository.Add(testPlan);
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
