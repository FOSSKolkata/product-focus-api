using CSharpFunctionalExtensions;
using MediatR;
using ProductTests.Domain.Common;
using ProductTests.Domain.Model.TestCaseVersionAggregate;
using ProductTests.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProductTests.Application.CommandHandler.TestRunCommands
{
    public sealed class MarkTestCasesVersionCommand : IRequest<Result>
    {
        public List<MarkTestCaseVersionDto> UpdatedTestCases { get; private set; }
        public MarkTestCasesVersionCommand(List<MarkTestCaseVersionDto> updatedTestCases)
        {
            UpdatedTestCases = updatedTestCases;
        }
        internal class MarkTestCasesVersionCommandHandler : IRequestHandler<MarkTestCasesVersionCommand, Result>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ITestCaseVersionRepository _testCaseVersionRepository;
            public MarkTestCasesVersionCommandHandler(IUnitOfWork unitOfWork, ITestCaseVersionRepository testCaseVersionRepository)
            {
                _unitOfWork = unitOfWork;
                _testCaseVersionRepository = testCaseVersionRepository;
            }
            public async Task<Result> Handle(MarkTestCasesVersionCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    foreach (var updatedTestCase in request.UpdatedTestCases)
                    {
                        TestCaseVersion testCase = await _testCaseVersionRepository.GetById(updatedTestCase.Id);
                        testCase.IncludeTestCast(updatedTestCase.IsSelected);
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
