using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Domain
{
    public interface IQuery<TResult>
    {
    }

    public interface IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        Task<TResult> Handle(TQuery query);
    }
}
