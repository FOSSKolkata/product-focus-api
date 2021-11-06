﻿using Microsoft.EntityFrameworkCore;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Persistence.Repositories
{
    class ModuleRepository : IModuleRepository
    {
        private UnitOfWork _unitOfWork;
        public ModuleRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<Module> GetById(long id)
        {
            return _unitOfWork.GetAsync<Module>(id);
        }
        public async Task<Module> GetByName(string name)
        {
            return await _unitOfWork.Query<Module>().Where(a => a.Name == name).SingleOrDefaultAsync();
        }
    }
}
