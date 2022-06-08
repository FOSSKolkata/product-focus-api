using CSharpFunctionalExtensions;
using MediatR;
using ProductTests.Domain.Common;
using ProductTests.Domain.Model.TestRunAggregate;
using ProductTests.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductTests.Application.CommandHandler.TestRunCommands
{
    public class MarkTestStepVersionStatusCommand: IRequest<Result>
    {
        public long TestRunId { get; private set; }
        public long Id { get; private set; }
        public TestStepResult ResultStatus { get; private set; }
        public MarkTestStepVersionStatusCommand(long testRunId, long id, TestStepResult resultStatus)
        {
            TestRunId = testRunId;
            Id = id;
            ResultStatus = resultStatus;
        }
        internal class MarkTestStepVersionStatusCommandHandler : IRequestHandler<MarkTestStepVersionStatusCommand, Result>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ITestRunRepository _testRunRepository;
            public MarkTestStepVersionStatusCommandHandler(IUnitOfWork unitOfWork, ITestRunRepository testRunRepository)
            {
                _unitOfWork = unitOfWork;
                _testRunRepository = testRunRepository;
            }

            public async Task<Result> Handle(MarkTestStepVersionStatusCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    TestRun testRun = await _testRunRepository.GetById(request.TestRunId);
                    testRun.UpdateTestStep(request.Id, request.ResultStatus);
                    await _unitOfWork.CompleteAsync(cancellationToken);
                    return Result.Success();
                }
                catch(Exception)
                {
                    throw;
                }
            }
        }
    }
}
