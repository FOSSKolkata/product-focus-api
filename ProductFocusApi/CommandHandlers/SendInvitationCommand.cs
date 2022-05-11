using CSharpFunctionalExtensions;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using ProductFocus.Services;
using System;
using System.Threading.Tasks;
using ProductFocus.Domain.Common;
using MediatR;
using System.Threading;

namespace ProductFocus.AppServices
{
    public sealed class SendInvitationCommand : IRequest<Result>
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

        internal sealed class SendInvitationCommandHandler : IRequestHandler<SendInvitationCommand, Result>
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
            public async Task<Result> Handle(SendInvitationCommand request, CancellationToken cancellationToken)
            {
                Organization existingOrganization = await _organizationRepository.GetById(request.OrgId);

                if (existingOrganization == null)
                    return Result.Failure($"Organization doesn't exist with id : '{request.OrgId}'");

                User existingUser = _userRepository.GetByEmail(request.Email);
                
                if(existingUser != null)
                {
                    bool ifUserAlreadyMember = existingOrganization.IfAlreadyMember(existingUser);

                    if (ifUserAlreadyMember)
                        return Result.Failure($"User '{request.Email}' is already a member of Organization id : '{request.OrgId}'");
                }

                Invitation existingActiveInvitation = _invitationRepository.GetActiveInvitation(existingOrganization, request.Email);

                if (existingActiveInvitation != null)
                    return Result.Failure($"Already an active invitation exists for '{request.Email}'.");          
                
                try
                {
                    User createdBy = _userRepository.GetByIdpUserId(request.ObjectId);
                    var invitation = Invitation.CreateInstance(existingOrganization, request.Email, createdBy.Id);
                    _invitationRepository.AddInvitation(invitation);           
                    
                    await _unitOfWork.CompleteAsync(cancellationToken);

                    Invitation newActiveInvitation = _invitationRepository.GetActiveInvitation(existingOrganization, request.Email);

                    string emailBody = $@"
                    Hi,
                    You are invited to join {existingOrganization.Name} on Product Focus by...
                    Click on following link to accept the invitation: 
                    https://productfocus.z13.web.core.windows.net/#/invitation?iid={newActiveInvitation.Id}";

                    _emailService.Send(emailBody, request.Email);

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
