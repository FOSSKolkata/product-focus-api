using ProductFocus.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Repositories
{
    public interface ISprintRepository
    {
        void AddSprint(Sprint sprint);
        Sprint GetByName(string name);
        Task<Sprint> GetById(long id);
    }
}
