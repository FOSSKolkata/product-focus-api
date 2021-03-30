using System;
using Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductFocus.Domain.Model;

namespace ProductFocus.Domain.Repositories
{
    public interface IRoleRepository<TEntity, TId> where TEntity : AggregateRoot<TId>
    {
    }
}
