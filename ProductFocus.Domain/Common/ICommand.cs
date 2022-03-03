using CSharpFunctionalExtensions;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Common
{
    public interface ICommand
    {
    }

    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        Task<Result> Handle(TCommand command);
    }
}
