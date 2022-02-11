using CSharpFunctionalExtensions;
using ProductFocus.Domain.Common;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Threading.Tasks;
using MediatR;
using System.Threading;

namespace ProductFocus.AppServices
{
    public sealed class CancelInvitationCommand : IRequest<Result>
    {
        public long InvitationId { get; set; }
        public long OrgId { get; set; }
        public string Email { get; }              
        public CancelInvitationCommand(long invitationId, long orgId, string email)
        {
            InvitationId = invitationId;
            Email = email;
            OrgId = orgId;
        }

        internal sealed class CancelInvitationCommandHandler : IRequestHandler<CancelInvitationCommand, Result>
        {
            private readonly IInvitationRepository _invitationRepository;                        
            private readonly IUnitOfWork _unitOfWork;
            

            public CancelInvitationCommandHandler(
                IInvitationRepository invitationRepository,                
                IUnitOfWork unitOfWork)
            {
                _invitationRepository = invitationRepository;                
                _unitOfWork = unitOfWork;                
            }
            public async Task<Result> Handle(CancelInvitationCommand request, CancellationToken cancellationToken)
            {
                try 
                {                     
                    Invitation existingActiveInvitation = await _invitationRepository.GetById(request.InvitationId);

                    if (existingActiveInvitation == null)
                        return Result.Failure($"No invitation exists for invitation id :'{request.InvitationId}'.");

                    //Start ---- Check if the invitation is matching with the email and organization 
                    if (existingActiveInvitation.Email != request.Email)
                        return Result.Failure($"Email sent over request parameter is not matching with the one in the invitation - invitatio id: '{request.InvitationId}'");

                    if (existingActiveInvitation.Organization.Id != request.OrgId)
                        return Result.Failure($"Organization sent is not matching with the organization present against invitation id: '{request.InvitationId}'");
                    //End ---- Check if the invitation is matching with the email and organization 

                    existingActiveInvitation.UpdateInvitationAsCancelled();
                    
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
