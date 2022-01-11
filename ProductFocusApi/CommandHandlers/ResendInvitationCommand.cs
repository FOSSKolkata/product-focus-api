using CSharpFunctionalExtensions;
using ProductFocus.Domain;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using ProductFocus.Services;
using System;
using System.Threading.Tasks;

namespace ProductFocusApi.CommandHandlers
{
    public class ResendInvitationCommand : ICommand
    {
        public long InvitationId { get; }
        public string ObjectId { get; }
        public ResendInvitationCommand(long invitationId, string objectId)
        {
            InvitationId = invitationId;
            ObjectId = objectId;
        }
        internal sealed class ResendInvitationCommandHandler : ICommandHandler<ResendInvitationCommand>
        {
            private readonly IInvitationRepository _invitationRepository;
            private readonly IUserRepository _userRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IEmailService _emailService;
            public ResendInvitationCommandHandler(
                IInvitationRepository invitationRepository,
                IUserRepository userRepository,
            IUnitOfWork unitOfWork, IEmailService emailService)
            {
                _invitationRepository = invitationRepository;
                _userRepository = userRepository;
                _unitOfWork = unitOfWork;
                _emailService = emailService;
            }
            public async Task<Result> Handle(ResendInvitationCommand command)
            {
                Invitation existingInvitation = await _invitationRepository.GetById(command.InvitationId);
                if (existingInvitation == null)
                    return Result.Failure("Invitation doesn't exist.");
                if (existingInvitation.Status == InvitationStatus.Accepted)
                    return Result.Failure($@"You are already a member of {existingInvitation.Organization.Name}.");
                try
                {

                    User modifiedBy = _userRepository.GetByIdpUserId(command.ObjectId);
                    existingInvitation.LastModifiedBy = modifiedBy.Name;
                    existingInvitation.Status = InvitationStatus.Resent;

                    await _unitOfWork.CompleteAsync();
                    string emailBody = $@"
                        Hi,
                        You are invited to join {existingInvitation.Organization.Name} on Product Focus by...
                        Click on following link to accept the invitation: 
                        http://localhost:4200/#/invitation?iid={existingInvitation.Id}";

                    _emailService.send(emailBody, existingInvitation.Email);

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
