using CSharpFunctionalExtensions;
using ProductFocus.Domain;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using ProductFocus.Services;
using System;
using System.Threading.Tasks;

namespace ProductFocus.AppServices
{
    public sealed class SendInvitationCommand : ICommand
    {
        public long OrgId { get; set; }
        public string Email { get; }
        public string ObjectId { get; }
        public SendInvitationCommand(long orgId, string email, string objectId)
        {
            Email = email;
            OrgId = orgId;
            ObjectId = objectId;
        }

        internal sealed class SendInvitationCommandHandler : ICommandHandler<SendInvitationCommand>
        {
            private readonly IInvitationRepository _invitationRepository;
            private readonly IOrganizationRepository _organizationRepository;
            private readonly IUserRepository _userRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IEmailService _emailService;

            public SendInvitationCommandHandler(
                IInvitationRepository invitationRepository,
                IOrganizationRepository organizationRepository,
                IUserRepository userRepository,
            IUnitOfWork unitOfWork, IEmailService emailService)
            {
                _invitationRepository = invitationRepository;
                _organizationRepository = organizationRepository;
                _userRepository = userRepository;
                _unitOfWork = unitOfWork;
                _emailService = emailService;
            }
            public async Task<Result> Handle(SendInvitationCommand command)
            {
                Organization existingOrganization = await _organizationRepository.GetById(command.OrgId);

                if (existingOrganization == null)
                    return Result.Failure($"Organization doesn't exist with id : '{command.OrgId}'");

                User existingUser = _userRepository.GetByEmail(command.Email);
                
                if(existingUser != null)
                {
                    bool ifUserAlreadyMember = existingOrganization.IfAlreadyMember(existingUser);

                    if (ifUserAlreadyMember)
                        return Result.Failure($"User '{command.Email}' is already a member of Organization id : '{command.OrgId}'");
                }

                Invitation existingActiveInvitation = _invitationRepository.GetActiveInvitation(existingOrganization, command.Email);

                if (existingActiveInvitation != null)
                    return Result.Failure($"Already an active invitation exists for '{command.Email}'.");          
                
                try
                {
                    User createdBy = _userRepository.GetByIdpUserId(command.ObjectId);
                    var invitation = Invitation.CreateInstance(existingOrganization, command.Email, createdBy.Id);
                    _invitationRepository.AddInvitation(invitation);           
                    
                    await _unitOfWork.CompleteAsync();

                    Invitation newActiveInvitation = _invitationRepository.GetActiveInvitation(existingOrganization, command.Email);

                    string emailBody = $@"
                    Hi,
                    You are invited to join {existingOrganization.Name} on Product Focus by...
                    Click on following link to accept the invitation: 
                    http://localhost:4200/#/invitation?iid={newActiveInvitation.Id}";

                    _emailService.Send(emailBody, command.Email);

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
