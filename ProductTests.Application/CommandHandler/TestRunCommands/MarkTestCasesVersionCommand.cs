using CSharpFunctionalExtensions;
using MediatR;
using ProductTests.Domain.Common;
using ProductTests.Domain.Model.TestRunAggregate;
using ProductTests.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProductTests.Application.CommandHandler.TestRunCommands
{
    public sealed class MarkTestCasesVersionCommand : IRequest<Result>
    {
        public long RunId { get; private set; }
        public List<MarkTestCaseVersionDto> UpdatedTestCases { get; private set; }
        public MarkTestCasesVersionCommand(long runId, List<MarkTestCaseVersionDto> updatedTestCases)
        {
            RunId = runId;
            UpdatedTestCases = updatedTestCases;
        }
        internal class MarkTestCasesVersionCommandHandler : IRequestHandler<MarkTestCasesVersionCommand, Result>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ITestRunRepository _testRunRepository;
            public MarkTestCasesVersionCommandHandler(IUnitOfWork unitOfWork, ITestRunRepository testRunRepository)
            {
                _unitOfWork = unitOfWork;
                _testRunRepository = testRunRepository;
            }
            public async Task<Result> Handle(MarkTestCasesVersionCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    TestRun testRun = await _testRunRepository.GetById(request.RunId);

                    foreach (var updatedTestCase in request.UpdatedTestCases)
                    {
                        testRun.IncludeTestCase(updatedTestCase.Id, updatedTestCase.IsSelected);
                    }
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
