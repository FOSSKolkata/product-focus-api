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
    public sealed class RegisterUserCommand : ICommand
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

        internal sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>
        {
            private readonly IUserRepository _userRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IEmailService _emailService;

            public RegisterUserCommandHandler(
                IUserRepository userRepository, IUnitOfWork unitOfWork, IEmailService emailService)
            {
                _userRepository = userRepository;
                _unitOfWork = unitOfWork;
                _emailService = emailService;
            }
            public async Task<Result> Handle(RegisterUserCommand command)
            {
                User existingUserWithSameEmail = _userRepository.GetByEmail(command.Email);

                if (existingUserWithSameEmail != null)
                    return Result.Failure($"User wih email '{command.Email}' already exists");

                try
                {
                    var user = User.CreateInstance(command.Name, command.Email, command.ObjectId);
                    _userRepository.RegisterUser(user);

                    await _unitOfWork.CompleteAsync();

                    _emailService.send();

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
