﻿using System.Threading.Tasks;

namespace ProductTests.Domain.Common
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
