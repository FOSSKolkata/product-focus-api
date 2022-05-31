using CSharpFunctionalExtensions;
using MediatR;
using ProductTests.Domain.Common;
using ProductTests.Domain.Model.TestCaseVersionAggregate;
using ProductTests.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductTests.Application.CommandHandler.TestRunCommands
{
    public class MarkTestCaseVersionStatusCommand : IRequest<Result>
    {
        public long Id { get; private set; }
        public TestCaseResult ResultStatus { get; private set; }
        public MarkTestCaseVersionStatusCommand(long id, TestCaseResult resultStatus)
        {
            Id = id;
            ResultStatus = resultStatus;
        }
        internal class MarkTestCaseVersionStatusCommandHandler : IRequestHandler<MarkTestCaseVersionStatusCommand, Result>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ITestCaseVersionRepository _testCaseVersionRepository;
            public MarkTestCaseVersionStatusCommandHandler(IUnitOfWork unitOfWork, ITestCaseVersionRepository testCaseVersionRepository)
            {
                _unitOfWork = unitOfWork;
                _testCaseVersionRepository = testCaseVersionRepository;
            }

            public async Task<Result> Handle(MarkTestCaseVersionStatusCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    TestCaseVersion testCase = await _testCaseVersionRepository.GetById(request.Id);
                    testCase.UpdateResultStatus(request.ResultStatus);
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
