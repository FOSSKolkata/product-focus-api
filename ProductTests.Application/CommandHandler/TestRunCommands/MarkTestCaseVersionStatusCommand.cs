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
    public class MarkTestCaseVersionStatusCommand : IRequest<Result>
    {
        public long RunId { get; private set; }
        public long Id { get; private set; }
        public TestCaseResult ResultStatus { get; private set; }
        public MarkTestCaseVersionStatusCommand(long runId, long id, TestCaseResult resultStatus)
        {
            RunId = runId;
            Id = id;
            ResultStatus = resultStatus;
        }
        internal class MarkTestCaseVersionStatusCommandHandler : IRequestHandler<MarkTestCaseVersionStatusCommand, Result>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ITestRunRepository _testRunRepository;
            public MarkTestCaseVersionStatusCommandHandler(IUnitOfWork unitOfWork, ITestRunRepository testRunRepository)
            {
                _unitOfWork = unitOfWork;
                _testRunRepository = testRunRepository;
            }

            public async Task<Result> Handle(MarkTestCaseVersionStatusCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    TestRun testRun = await _testRunRepository.GetById(request.RunId);
                    testRun.UpdateResultStatus(request.Id, request.ResultStatus);

                    await _unitOfWork.CompleteAsync(cancellationToken);
                    return Result.Success();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
