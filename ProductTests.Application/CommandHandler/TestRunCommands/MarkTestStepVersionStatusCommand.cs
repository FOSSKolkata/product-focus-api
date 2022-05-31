using CSharpFunctionalExtensions;
using MediatR;
using ProductTests.Domain.Common;
using ProductTests.Domain.Model.TestCaseVersionAggregate;
using ProductTests.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProductTests.Application.CommandHandler.TestRunCommands
{
    public class MarkTestStepVersionStatusCommand: IRequest<Result>
    {
        public long TestCaseId { get; private set; }
        public long Id { get; private set; }
        public TestStepResult ResultStatus { get; private set; }
        public MarkTestStepVersionStatusCommand(long testCaseId, long id, TestStepResult resultStatus)
        {
            TestCaseId = testCaseId;
            Id = id;
            ResultStatus = resultStatus;
        }
        internal class MarkTestStepVersionStatusCommandHandler : IRequestHandler<MarkTestStepVersionStatusCommand, Result>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ITestCaseVersionRepository _testCaseVersionRepository;
            public MarkTestStepVersionStatusCommandHandler(IUnitOfWork unitOfWork, ITestCaseVersionRepository testCaseVersionRepository)
            {
                _unitOfWork = unitOfWork;
                _testCaseVersionRepository = testCaseVersionRepository;
            }

            public async Task<Result> Handle(MarkTestStepVersionStatusCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    TestCaseVersion testCase = await _testCaseVersionRepository.GetById(request.TestCaseId);
                    testCase.UpdateTestStep(request.Id, request.ResultStatus);
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
