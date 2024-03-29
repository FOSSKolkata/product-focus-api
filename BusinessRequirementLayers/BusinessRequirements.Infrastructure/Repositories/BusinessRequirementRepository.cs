﻿using BusinessRequirements.Domain.Model;
using BusinessRequirements.Domain.Repositories;
using BusinessRequirements.Infrastructure;

namespace ProductFocus.Persistence.Repositories
{
    public class BusinessRequirementRepository : IBusinessRequirementRepository
    {
        private readonly UnitOfWork _unitOfWork;
        public BusinessRequirementRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void Add(BusinessRequirement businessRequirement)
        {
            _unitOfWork.Insert(businessRequirement);
        }

        public Task<BusinessRequirement> GetById(long id)
        {
            return _unitOfWork.GetAsync<BusinessRequirement>(id);
        }
    }
}
