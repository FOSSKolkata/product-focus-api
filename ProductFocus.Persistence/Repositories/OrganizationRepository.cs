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
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly UnitOfWork _unitOfWork;

        public OrganizationRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddOrganization(Organization organization)
        {
            _unitOfWork.InsertAsync<Organization>(organization);
        }

        public async Task<Organization> GetById(long id)
        {
            return await _unitOfWork.GetAsync<Organization>(id);
        }

        public Organization GetByName(string name)
        {
            return _unitOfWork.Query<Organization>().SingleOrDefault(x => x.Name == name);
        }
    }
}