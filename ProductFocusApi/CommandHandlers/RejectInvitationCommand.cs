﻿using Common;
using CSharpFunctionalExtensions;
using ProductFocus.Domain;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using ProductFocus.Services;
using System;
using System.Threading.Tasks;

namespace ProductFocus.AppServices
{
    public sealed class RejectInvitationCommand : ICommand
    {
        public long InvitationId { get; set; }
        public long OrgId { get; set; }
        public string Email { get; }              
        public RejectInvitationCommand(long invitationId, long orgId, string email)
        {
            InvitationId = invitationId;
            Email = email;
            OrgId = orgId;
        }

        internal sealed class RejectInvitationCommandHandler : ICommandHandler<RejectInvitationCommand>
        {
            private readonly IInvitationRepository _invitationRepository;                        
            private readonly IUnitOfWork _unitOfWork;
            

            public RejectInvitationCommandHandler(
                IInvitationRepository invitationRepository,                
                IUnitOfWork unitOfWork)
            {
                _invitationRepository = invitationRepository;                
                _unitOfWork = unitOfWork;                
            }
            public async Task<Result> Handle(RejectInvitationCommand command)
            {
                try 
                {                     
                    Invitation existingActiveInvitation = await _invitationRepository.GetById(command.InvitationId);

                    if (existingActiveInvitation == null)
                        return Result.Failure($"No invitation exists for invitation id :'{command.InvitationId}'.");

                    //Start ---- Check if the invitation is matching with the email and organization 
                    if (existingActiveInvitation.Email != command.Email)
                        return Result.Failure($"Email sent over request parameter is not matching with the one in the invitation - invitatio id: '{command.InvitationId}'");

                    if (existingActiveInvitation.Organization.Id != command.OrgId)
                        return Result.Failure($"Organization sent is not matching with the organization present against invitation id: '{command.InvitationId}'");
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