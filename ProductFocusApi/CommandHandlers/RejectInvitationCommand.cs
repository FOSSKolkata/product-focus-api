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
    public sealed class RejectInvitationCommand : IRequest<Result>
    {
        public long InvitationId { get; set; }
        public string ObjectId { get; }              
        public RejectInvitationCommand(long invitationId, string objectId)
        {
            InvitationId = invitationId;
            ObjectId = objectId;
        }

        internal sealed class RejectInvitationCommandHandler : IRequestHandler<RejectInvitationCommand, Result>
        {
            private readonly IInvitationRepository _invitationRepository;                        
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUserRepository _userRepository;
            private readonly IOrganizationRepository _organizationRepository;
            

            public RejectInvitationCommandHandler(
                IInvitationRepository invitationRepository,                
                IUnitOfWork unitOfWork,
                IUserRepository userRepository,
                IOrganizationRepository organizationRepository)
            {
                _invitationRepository = invitationRepository;                
                _unitOfWork = unitOfWork;
                _userRepository = userRepository;
                _organizationRepository = organizationRepository;
            }
            public async Task<Result> Handle(RejectInvitationCommand request, CancellationToken cancellationToken)
            {
                try 
                {
                    User existingUser = _userRepository.GetByIdpUserId(request.ObjectId);

                    if (existingUser == null)
                        return Result.Failure($"User are not a registered user.");

                    Invitation existingActiveInvitation = await _invitationRepository.GetById(request.InvitationId);

                    Organization existingOrganization = await _organizationRepository.GetById(existingActiveInvitation.Organization.Id);

                    if (existingActiveInvitation == null)
                        return Result.Failure($"No invitation exists for invitation id :'{request.InvitationId}'.");

                    //Start ---- Check if the invitation is matching with the email and organization 
                    if (existingActiveInvitation.Email != existingUser.Email)
                        return Result.Failure($"Email sent over request parameter is not matching with the one in the invitation - invitatio id: '{request.InvitationId}'");

                    if (existingActiveInvitation.Organization.Id != existingOrganization.Id)
                        return Result.Failure($"Organization sent is not matching with the organization present against invitation id: '{request.InvitationId}'");
                    //End ---- Check if the invitation is matching with the email and organization 

                    existingActiveInvitation.UpdateInvitationAsRejected();
                    
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
