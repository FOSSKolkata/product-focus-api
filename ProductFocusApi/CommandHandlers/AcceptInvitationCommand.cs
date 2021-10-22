using Common;
using CSharpFunctionalExtensions;
using ProductFocus.Domain;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using ProductFocus.Services;
using System;
using System.Threading.Tasks;

namespace ProductFocus.AppServices
{
    public sealed class AcceptInvitationCommand : ICommand
    {
        public long InvitationId { get; set; }
        public string ObjectId { get; }
        public AcceptInvitationCommand(long invitationId, string objectId)
        {
            InvitationId = invitationId;
            ObjectId = objectId;
        }

        internal sealed class AcceptInvitationCommandHandler : ICommandHandler<AcceptInvitationCommand>
        {
            private readonly IInvitationRepository _invitationRepository;
            private readonly IOrganizationRepository _organizationRepository;
            private readonly IUserRepository _userRepository;
            private readonly IUnitOfWork _unitOfWork;

            public AcceptInvitationCommandHandler(
                IInvitationRepository invitationRepository,
                IOrganizationRepository organizationRepository,
                IUserRepository userRepository,
            IUnitOfWork unitOfWork)
            {
                _invitationRepository = invitationRepository;
                _organizationRepository = organizationRepository;
                _userRepository = userRepository;
                _unitOfWork = unitOfWork;                
            }
            public async Task<Result> Handle(AcceptInvitationCommand command)
            {
                try 
                {
                    User existingUser = _userRepository.GetByIdpUserId(command.ObjectId);
                
                    if(existingUser == null)
                        return Result.Failure($"User are not a registered user.");

                    Invitation existingActiveInvitation = await _invitationRepository.GetById(command.InvitationId);

                    Organization existingOrganization = await _organizationRepository.GetById(existingActiveInvitation.Organization.Id);

                    if (existingOrganization == null)
                        return Result.Failure($"Organization doesn't exist with id : '{existingActiveInvitation.Organization.Id}'");
                    if (existingActiveInvitation == null)
                        return Result.Failure($"No invitation exists for invitation id :'{command.InvitationId}'.");

                    //Start ---- Check if the invitation is matching with the email and organization 
                    if (existingActiveInvitation.Email != existingUser.Email)
                        return Result.Failure($"Email sent over request parameter is not matching with the one in the invitation - invitatio id: '{command.InvitationId}'");

                    if (existingActiveInvitation.Organization != existingOrganization)
                        return Result.Failure($"Organization sent is not matching with the organization present against invitation id: '{command.InvitationId}'");
                    //End ---- Check if the invitation is matching with the email and organization 
                                        
                    existingOrganization.AddMember(existingUser, false);

                    existingActiveInvitation.UpdateInvitationAsAccepted();
                    
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
