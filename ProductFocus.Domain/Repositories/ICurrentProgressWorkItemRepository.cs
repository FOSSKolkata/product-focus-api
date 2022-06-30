using ProductFocus.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Repositories
{
    public interface ICurrentProgressWorkItemRepository
    {
        void Add(CurrentProgressWorkItem currentProgressWorkItem);
        Task<CurrentProgressWorkItem> GetById(long id);
        Task<List<CurrentProgressWorkItem>> GetAllUserItemByProductId(long productId, long userId);
    }
}
