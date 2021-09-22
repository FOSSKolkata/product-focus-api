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
    public class UserRepository : IUserRepository
    {
        private readonly UnitOfWork _unitOfWork;

        public UserRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public User GetByIdpUserId(string idpUserId)
        {
            return _unitOfWork.Query<User>().SingleOrDefault(x => x.ObjectId == idpUserId);
        }

        public User GetByEmail(string email)
        {
            return _unitOfWork.Query<User>().SingleOrDefault(x => x.Email == email);
        }

        public void RegisterUser(User user)
        {
            _unitOfWork.Insert<User>(user);
        }
        public User GetById(long id)
        {
            return _unitOfWork.Query<User>().SingleOrDefault(x => x.Id == id);
        }
    }
}
