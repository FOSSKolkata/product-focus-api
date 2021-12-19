using CSharpFunctionalExtensions;
using ProductFocus.Domain;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using ProductFocus.Services;
using System;
using System.Threading.Tasks;

namespace ProductFocus.AppServices
{
    public sealed class AddOrganizationCommand : ICommand
    {
        public string OrganizationName { get; }      
        public string IdpUserId { get; set; }
        public AddOrganizationCommand(string name, string idpUserId)
        {
            OrganizationName = name;
            IdpUserId = idpUserId;
        }

        internal sealed class AddOrganizationCommandHandler : ICommandHandler<AddOrganizationCommand>
        {
            private readonly IOrganizationRepository _organizationRepository;
            private readonly IUserRepository _userRepository;
            private readonly IUnitOfWork _unitOfWork;

            public AddOrganizationCommandHandler(
                IOrganizationRepository organizationRepository, IUserRepository userRepository,
                IUnitOfWork unitOfWork)
            {
                _organizationRepository = organizationRepository;
                _userRepository = userRepository;
                _unitOfWork = unitOfWork;
            }
            public async Task<Result> Handle(AddOrganizationCommand command)
            {
                Organization existingOrganizationWithSameName = _organizationRepository.GetByName(command.OrganizationName);

                if (existingOrganizationWithSameName != null)
                    return Result.Failure($"Organization '{command.OrganizationName}' already exists");
                
                var user = _userRepository.GetByIdpUserId(command.IdpUserId);

                if (user == null)
                    return Result.Failure($"User with IdpUserId '{command.IdpUserId}' doesn't exist");

                try
                {
                    var organization = Organization.CreateInstance(command.OrganizationName);
                    _organizationRepository.AddOrganization(organization);
                    
                    organization.AddMember(user, true);

                    await _unitOfWork.CompleteAsync();

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
