using CSharpFunctionalExtensions;
using MediatR;
using Releases.Domain.Common;
using Releases.Domain.Model;
using Releases.Domain.Repositories;

namespace Releases.Application.CommandHandler.ReleaseCommands
{
    public sealed class AddReleaseCommand : IRequest<Result>
    {
        public long ProductId { get; private set; }
        public string Name { get; private set; }
        public DateTime? ReleaseDate { get; private set; }
        public AddReleaseCommand(long productId, string name, DateTime releaseDate)
        {
            ProductId = productId;
            Name = name;
            ReleaseDate = releaseDate;
        }
        internal sealed class AddReleaseCommandHandler : IRequestHandler<AddReleaseCommand, Result>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IReleaseRepository _releaseRepository;
            public AddReleaseCommandHandler(IUnitOfWork unitOfWork, IReleaseRepository releaseRepository)
            {
                _unitOfWork = unitOfWork;
                _releaseRepository = releaseRepository;
            }

            public async Task<Result> Handle(AddReleaseCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    Release release = Release.CreateInstance(request.ProductId, request.Name, request.ReleaseDate);
                    _releaseRepository.Add(release);
                    await _unitOfWork.CompleteAsync(cancellationToken);
                } catch(Exception)
                {
                    throw;
                }
                return Result.Success();
            }
        }
    }
}
