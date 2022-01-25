using CSharpFunctionalExtensions;
using ProductFocus.Domain;
using System;
using System.Threading.Tasks;

namespace ProductDocumentations.CommandHandlers
{
    public sealed class AddProductDocumentationCommand : ICommand
    {
        public long? ParentId { get; private set; }
        public long ProductId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }

        public AddProductDocumentationCommand(long? parentId, long productId, string title, string description)
        {
            ParentId = parentId;
            ProductId = productId;
            Title = title;
            Description = description;
        }

        public sealed class AddProductDocumentationCommandHandler : ICommandHandler<AddProductDocumentationCommand>
        {
            private readonly IUnitOfWork _unitOfWork;
            public AddProductDocumentationCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }
            public async Task<Result> Handle(AddProductDocumentationCommand command)
            {
                throw new NotImplementedException();
            }
        }
    }
}
