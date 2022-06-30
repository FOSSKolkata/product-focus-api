using CSharpFunctionalExtensions;
using MediatR;
using ProductFocus.Domain.Common;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using ProductFocusApi.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductFocusApi.CommandHandlers
{
    public class AddCurrentProgressWorkItemCommand : IRequest<Result<GetCurrentProgressWorkItemDto>>
    {
        public long ProductId { get; private set; }
        public string UserObjectId { get; private set; }
        public long WorkItemId { get; private set; }
        public long? PreviouslyProgressWorkItem { get; private set; }
        public AddCurrentProgressWorkItemCommand(long productId, long workItemId, string userObjectId, long? previouslyProgressWorkItem)
        {
            ProductId = productId;
            UserObjectId = userObjectId;
            WorkItemId = workItemId;
            PreviouslyProgressWorkItem = previouslyProgressWorkItem;
        }
        internal class AddCurrentProgressWorkItemCommandHander : IRequestHandler<AddCurrentProgressWorkItemCommand, Result<GetCurrentProgressWorkItemDto>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ICurrentProgressWorkItemRepository _currentProgressWorkItemRepository;
            private readonly IUserRepository _userRepository;
            public AddCurrentProgressWorkItemCommandHander(IUnitOfWork unitOfWork, ICurrentProgressWorkItemRepository currentProgressWorkItemRepository,
                IUserRepository userRepository)
            {
                _unitOfWork = unitOfWork;
                _currentProgressWorkItemRepository = currentProgressWorkItemRepository;
                _userRepository = userRepository;
            }

            public async Task<Result<GetCurrentProgressWorkItemDto>> Handle(AddCurrentProgressWorkItemCommand request, CancellationToken cancellationToken)
            {
                CurrentProgressWorkItem currentProgressWorkItem;
                try
                {
                    User user = _userRepository.GetByIdpUserId(request.UserObjectId);
                    if (request.PreviouslyProgressWorkItem.HasValue)
                    {
                        CurrentProgressWorkItem previouslyProgressWorkItem = await _currentProgressWorkItemRepository.GetById(request.PreviouslyProgressWorkItem.Value);
                        previouslyProgressWorkItem.Delete(user.Name);
                    }
                    var previousItems = await _currentProgressWorkItemRepository.GetAllUserItemByProductId(request.ProductId, user.Id);
                    foreach (var item in previousItems)
                    {
                        item.Delete(user.Name);
                    }
                    currentProgressWorkItem = CurrentProgressWorkItem.CreateInstance(
                        request.ProductId, request.WorkItemId, user.Id);
                    _currentProgressWorkItemRepository.Add(currentProgressWorkItem);
                    await _unitOfWork.CompleteAsync(cancellationToken);
                    GetCurrentProgressWorkItemDto dto = new();
                    dto.WorkItemId = currentProgressWorkItem.WorkItemId;
                    dto.Id = currentProgressWorkItem.Id;
                    return Result.Success(dto);
                } catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
