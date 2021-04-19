using Common;
using CSharpFunctionalExtensions;
using ProductFocus.Domain;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using ProductFocus.Services;
using System.Threading.Tasks;

namespace ProductFocus.AppServices
{
    public sealed class AddOrganizationCommand : ICommand
    {
        public string Name { get; }
        public AddOrganizationCommand(string name)
        {
            Name = name;
        }

        internal sealed class AddOrganizationCommandHandler : ICommandHandler<AddOrganizationCommand>
        {
            private readonly IOrganizationRepository _organizationRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IEmailService _emailService;

            public AddOrganizationCommandHandler(
                IOrganizationRepository organizationRepository, IUnitOfWork unitOfWork, IEmailService emailService)
            {
                _organizationRepository = organizationRepository;
                _unitOfWork = unitOfWork;
                _emailService = emailService;
            }
            public async Task<Result> Handle(AddOrganizationCommand command)
            {
                Organization seachExisingOrganization = _organizationRepository.GetByName(command.Name);
                if (seachExisingOrganization != null)
                    return Result.Failure($"Organization '{command.Name}' already exists");
                var organization = new Organization(command.Name);
                _organizationRepository.AddOrganization(organization);
                await _unitOfWork.CompleteAsync();
                _emailService.send();
                return Result.Success();
            }

        }
    }
}
