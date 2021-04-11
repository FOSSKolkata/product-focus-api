using Common;
using CSharpFunctionalExtensions;
using ProductFocus.Domain;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;


namespace ProductFocus.AppServices
{
    public sealed class AddOrganizationCommand : ICommand
    {
        public string Name { get; set; }
        public AddOrganizationCommand(string name)
        {
            Name = name;
        }

        internal sealed class AddOrganizationCommandHandler : ICommandHandler<AddOrganizationCommand>
        {
            private readonly IOrganizationRepository _organizationRepository;
            private readonly IUnitOfWork _unitOfWork;
            public AddOrganizationCommandHandler(
                IOrganizationRepository organizationRepository, IUnitOfWork unitOfWork)
            {
                _organizationRepository = organizationRepository;
                _unitOfWork = unitOfWork;
            }
            public Result Handle(AddOrganizationCommand command)
            {
                var organization = new Organization(command.Name);
                _organizationRepository.AddOrganization(organization);
                _unitOfWork.Complete();
                return Result.Success();
            }

        }
    }
}
