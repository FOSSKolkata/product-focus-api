using BusinessRequirements.Domain.Model;
using BusinessRequirements.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessRequirements.Infrastructure.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly UnitOfWork _unitOfWork;
        public TagRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void Add(Tag tag)
        {
            _unitOfWork.Insert(tag);
        }

        public Task<Tag> GetById(long id)
        {
            return _unitOfWork.GetAsync<Tag>(id);
        }
    }
}
