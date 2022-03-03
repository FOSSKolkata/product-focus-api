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
    public sealed class RegisterUserCommand : IRequest<Result>
    {
        public string Name { get; }
        public string Email { get; set; }
        public string ObjectId { get; set; }
        public RegisterUserCommand(string name, string email, string objectid)
        {
            Name = name;
            Email = email;
            ObjectId = objectid;
        }

        internal sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result>
        {
            private readonly IUserRepository _userRepository;
            private readonly IUnitOfWork _unitOfWork;

            public RegisterUserCommandHandler(
                IUserRepository userRepository, IUnitOfWork unitOfWork)
            {
                _userRepository = userRepository;
                _unitOfWork = unitOfWork;
            }
            public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
            {
                User existingUserWithSameEmail = _userRepository.GetByEmail(request.Email);

                if (existingUserWithSameEmail != null)
                    return Result.Failure($"User wih email '{request.Email}' already exists");

                try
                {
                    var user = User.CreateInstance(request.Name, request.Email, request.ObjectId);
                    _userRepository.RegisterUser(user);

                    await _unitOfWork.CompleteAsync(cancellationToken);

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
