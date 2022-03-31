using CSharpFunctionalExtensions;
using MediatR;
using ProductTests.Domain.Common;
using ProductTests.Domain.Model.TestCaseAggregate;
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
        public DeleteTestCaseCommand(long id, string userId)
        {
            Id = id;
            UserId = userId;
        }
        internal sealed class DeleteTestCaseCommandHandler : IRequestHandler<DeleteTestCaseCommand, Result>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ITestCaseRepository _testCaseRepository;
            public DeleteTestCaseCommandHandler(IUnitOfWork unitOfWork, ITestCaseRepository testCaseRepository)
            {
                _unitOfWork = unitOfWork;
                _testCaseRepository = testCaseRepository;
            }
            public async Task<Result> Handle(DeleteTestCaseCommand request, CancellationToken cancellationToken)
            {
                try
                {
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
