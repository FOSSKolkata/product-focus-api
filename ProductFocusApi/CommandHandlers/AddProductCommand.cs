using CSharpFunctionalExtensions;
using ProductFocus.Domain;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using ProductFocus.Services;
using System.Threading.Tasks;

namespace ProductFocus.AppServices
{
    public sealed class AddProductCommand : ICommand
    {
        public long Id { get; }
        public string Name { get; }
        public AddProductCommand(long id, string name)
        {
            Id = id;
            Name = name;
        }

        internal sealed class AddProductCommandHandler : ICommandHandler<AddProductCommand>
        {
            private readonly IOrganizationRepository _organizationRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IEmailService _emailService;

            public AddProductCommandHandler(
                IOrganizationRepository organizationRepository, IUnitOfWork unitOfWork, IEmailService emailService)
            {
                _organizationRepository = organizationRepository;
                _unitOfWork = unitOfWork;
                _emailService = emailService;
            }
            public async Task<Result> Handle(AddProductCommand command)
            {
                Organization organization = await _organizationRepository.GetById(command.Id);
                if (organization == null)
                    return Result.Failure($"No Organization found with Id '{command.Id}'");
                organization.AddProduct(command.Name);
                await _unitOfWork.CompleteAsync();
                _emailService.send();
                return Result.Success();
            }

        }
    }
}
