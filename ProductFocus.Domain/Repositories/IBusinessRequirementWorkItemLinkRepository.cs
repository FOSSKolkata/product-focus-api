using ProductFocus.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Repositories
{
    public interface IBusinessRequirementWorkItemLinkRepository
    {
        void Add(BusinessRequirementToWorkItemLink businessRequirementToWorkItemLink);
        Task<BusinessRequirementToWorkItemLink> GetById(long id);
    }
}
