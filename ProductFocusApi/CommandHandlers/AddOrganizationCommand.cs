using CSharpFunctionalExtensions;
using ProductFocus.Domain.Common;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Threading.Tasks;
using MediatR;
using System.Threading;
using ProductFocusApi.IntegrationEvents.Events;
using ProductFocusApi.IntegrationEvents.Services;
using ProductFocus.Persistence.DbContexts;
using ProductFocusApi.IntegrationCommands.Services;
using IntegrationCommon.Services;

namespace ProductFocus.AppServices
{
    public sealed class AddOrganizationCommand : IRequest<Result>
    {
        public string OrganizationName { get; }      
        public string IdpUserId { get; set; }
        public AddOrganizationCommand(string name, string idpUserId)
        {
            OrganizationName = name;
            IdpUserId = idpUserId;
        }

        internal sealed class AddOrganizationCommandHandler : IRequestHandler<AddOrganizationCommand, Result>
        {
            private readonly IOrganizationRepository _organizationRepository;
            private readonly IUserRepository _userRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IAtomicIntegrationLogService _atomicIntegrationLogService;
            private readonly IProductFocusIntegrationEventService _productFocusIntegrationEventService;
            public AddOrganizationCommandHandler(
                IOrganizationRepository organizationRepository, IUserRepository userRepository,
                IUnitOfWork unitOfWork,
                AtomicIntegrationLogService<ProductFocusContext,
                    ProductFocusIntegrationCommandLogService,
                    ProductFocusIntegrationEventLogService> atomicIntegrationLogService)
            {
                _organizationRepository = organizationRepository;
                _userRepository = userRepository;
                _unitOfWork = unitOfWork;
                _atomicIntegrationLogService = atomicIntegrationLogService;
            }
            public async Task<Result> Handle(AddOrganizationCommand request, CancellationToken cancellationToken)
            {
                Organization existingOrganizationWithSameName = _organizationRepository.GetByName(request.OrganizationName);

                if (existingOrganizationWithSameName != null)
                    return Result.Failure($"Organization '{request.OrganizationName}' already exists");
                
                var user = _userRepository.GetByIdpUserId(request.IdpUserId);

                if (user == null)
                    return Result.Failure($"User with IdpUserId '{request.IdpUserId}' doesn't exist");

                try
                {
                    var organization = Organization.CreateInstance(request.OrganizationName);
                    _organizationRepository.AddOrganization(organization);
                    
                    organization.AddMember(user, true);

                    //OrganizationAddedIntegrationEvent
                    OrganizationAddedIntegrationEvent @event = new(organization.Id, organization.Name);
                    await _atomicIntegrationLogService.SaveAtomicallyWithDbContextChangesAsync();
                    await _productFocusIntegrationEventService.PublishThroughEventBusAsync(@event);
                    //await _unitOfWork.CompleteAsync(cancellationToken);

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
