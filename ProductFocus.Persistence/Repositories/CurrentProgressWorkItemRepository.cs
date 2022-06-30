using Microsoft.EntityFrameworkCore;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Persistence.Repositories
{
    public class CurrentProgressWorkItemRepository : ICurrentProgressWorkItemRepository
    {
        private readonly UnitOfWork _unitOfWork;
        public CurrentProgressWorkItemRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Add(CurrentProgressWorkItem currentProgressWorkItem)
        {
            _unitOfWork.Insert(currentProgressWorkItem);
        }

        public Task<CurrentProgressWorkItem> GetById(long id)
        {
            return _unitOfWork.GetAsync<CurrentProgressWorkItem>(id);
        }

        public Task<List<CurrentProgressWorkItem>> GetAllUserItemByProductId(long productId, long userId)
        {
            return _unitOfWork.Query<CurrentProgressWorkItem>()
                .Where(a => a.ProductId == productId && a.UserId == userId && a.IsDeleted == false)
                .ToListAsync();
        }
    }
}
