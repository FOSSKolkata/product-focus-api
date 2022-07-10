using BusinessRequirements.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessRequirements.Domain.Repositories
{
    public interface ITagRepository
    {
        void Add(Tag tag);
        Task<Tag> GetById(long id);
    }
}
