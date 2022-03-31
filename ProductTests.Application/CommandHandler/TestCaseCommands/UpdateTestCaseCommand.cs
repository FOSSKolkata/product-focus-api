using CSharpFunctionalExtensions;
using MediatR;
using ProductTests.Domain.Common;
using ProductTests.Domain.Model.TestCaseAggregate;
using ProductTests.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProductTests.Application.CommandHandler.TestCaseCommands
{
    public sealed class UpdateTestCaseCommand : IRequest<Result>
    {
        public long Id { get; private set; }
        public string Title { get; private set; }
        public string Preconditions { get; private set; }
        public List<UpdateTestStepDto> TestSteps { get; private set; }
        public UpdateTestCaseCommand(long id, string title, string preconditions, List<UpdateTestStepDto> testSteps)
        {
            Id = id;
            Title = title;
            Preconditions = preconditions;
            TestSteps = testSteps;
        }
        internal sealed class UpdateTestCaseCommandHandler : IRequestHandler<UpdateTestCaseCommand, Result>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ITestCaseRepository _testCaseRepository;
            public UpdateTestCaseCommandHandler(IUnitOfWork unitOfWork, ITestCaseRepository testCaseRepository)
            {
                _unitOfWork = unitOfWork;
                _testCaseRepository = testCaseRepository;
            }
            public async Task<Result> Handle(UpdateTestCaseCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    TestCase testCase = await _testCaseRepository.GetById(request.Id);
                    testCase.UpdateTitle(request.Title);
                    testCase.UpdatePreconditions(request.Preconditions);
                    List<TestStep> testSteps = new();
                    for (int i = 0; i < request.TestSteps.Count; i++)
                    {
                        UpdateTestStepDto testStep = request.TestSteps[i];
                        if (testStep.Id != 0)
                            testSteps.Add(testStep.ToTestStep(testStep.Id, i + 1));
                        else
                            testSteps.Add(testStep.ToTestStep(0, i + 1));
                    }
                    testCase.UpdateTestSteps(testSteps);
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
