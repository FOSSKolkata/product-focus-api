using Common;
using CSharpFunctionalExtensions;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using ProductFocus.Persistence;
using ProductFocus.Persistence.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Persistence.AppServices
{
    public sealed class AddOrganizationCommand : ICommand
    {
        public string Name { get; set; }
        public AddOrganizationCommand(string name)
        {
            Name = name;
        }

        public sealed class AddOrganizationCommandHandler : ICommandHandler<AddOrganizationCommand>
        {
            private readonly ProductFocusDbContext _productFocusDbContext;
            private readonly IOrganizationRepository _organizationRepository;

            public AddOrganizationCommandHandler(
                ProductFocusDbContext productFocusDbContext,
                IOrganizationRepository organizationRepository)
            {
                _productFocusDbContext = productFocusDbContext;
                _organizationRepository = organizationRepository;
            }
            public Result Handle(AddOrganizationCommand command)
            {
                var unitOfWork = new UnitOfWork(_productFocusDbContext);
                var organization = new Organization(command.Name);
                _organizationRepository.AddOrganization(organization);
                unitOfWork.Complete();
                return Result.Success();
            }

        }
    }
}
