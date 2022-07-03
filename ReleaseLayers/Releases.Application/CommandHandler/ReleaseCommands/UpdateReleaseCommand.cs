using MediatR;
using Releases.Domain.Common;
using Releases.Domain.Model;
using Releases.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Releases.Application.CommandHandler.ReleaseCommands
{
    public class UpdateReleaseCommand : IRequest<Release>
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public DateTime ReleaseDate { get; private set; }
        public ReleaseStatus ReleaseStatus { get; private set; }
        public UpdateReleaseCommand(long id, string name, DateTime releaseDate, ReleaseStatus releaseStatus)
        {
            Id = id;
            Name = name;
            ReleaseDate = releaseDate;
            ReleaseStatus = releaseStatus;
        }
        internal class UpdateReleaseCommandHandler : IRequestHandler<UpdateReleaseCommand, Release>
        {
            private IUnitOfWork _unitOfWork;
            private IReleaseRepository _releaseRepository;
            public UpdateReleaseCommandHandler(IUnitOfWork unitOfWork, IReleaseRepository releaseRepository)
            {
                _unitOfWork = unitOfWork;
                _releaseRepository = releaseRepository;
            }

            public async Task<Release> Handle(UpdateReleaseCommand request, CancellationToken cancellationToken)
            {
                Release release;
                try
                {
                    release = await _releaseRepository.GetById(request.Id);
                    release.UpdateName(request.Name);
                    release.UpdateReleaseDate(request.ReleaseDate);
                    release.UpdateStatus(request.ReleaseStatus);
                    await _unitOfWork.CompleteAsync(cancellationToken);
                }catch(Exception)
                {
                    throw;
                }
                return release;
            }
        }
    }
}
