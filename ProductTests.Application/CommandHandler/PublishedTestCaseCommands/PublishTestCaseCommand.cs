using CSharpFunctionalExtensions;
using MediatR;
using ProductTests.Domain.Common;
using ProductTests.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace ProductTests.Application.CommandHandler.PublishedTestCaseCommands
{
    public sealed class PublishTestCaseCommand : IRequest<Result>
    {
        public long Id { get; private set; }
        public PublishTestCaseCommand(long id)
        {
            Id = id;
        }
        internal sealed class PublishTestCaseCommandHandler : IRequestHandler<PublishTestCaseCommand, Result>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IPublishedTestCaseRepository _publishedTestCaseRepository;
            public PublishTestCaseCommandHandler(IPublishedTestCaseRepository publishedTestCaseRepository,
                IUnitOfWork unitOfWork)
            {
                _publishedTestCaseRepository = publishedTestCaseRepository;
                _unitOfWork = unitOfWork;
            }
            public Task<Result> Handle(PublishTestCaseCommand request, CancellationToken cancellationToken)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
