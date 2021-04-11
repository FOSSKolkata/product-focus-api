using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Domain
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
