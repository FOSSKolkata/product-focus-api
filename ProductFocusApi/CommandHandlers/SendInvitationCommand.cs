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
    public sealed class SendInvitationCommand : ICommand
    {
        public string Email { get; }              
        public SendInvitationCommand(string email)
        {
            Email = email;
        }

        internal sealed class SendInvitationCommandHandler : ICommandHandler<SendInvitationCommand>
        {
            private readonly IInvitationRepository _invitationRepository;
            
            private readonly IUnitOfWork _unitOfWork;
            

            public SendInvitationCommandHandler(
                IInvitationRepository invitationRepository, 
                IUnitOfWork unitOfWork)
            {
                _invitationRepository = invitationRepository;                
                _unitOfWork = unitOfWork;                
            }
            public async Task<Result> Handle(SendInvitationCommand command)
            {
                Invitation existingInvitationWithSameEmail = _invitationRepository.GetByEmail(command.Email);

                if (existingInvitationWithSameEmail != null)
                    return Result.Failure($"Invitation for '{command.Email}' already sent");          
                
                try
                {
                    var invitation = Invitation.CreateInstance(command.Email);
                    _invitationRepository.AddInvitation(invitation);           
                    
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
