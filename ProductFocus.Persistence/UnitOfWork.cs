﻿using Microsoft.EntityFrameworkCore;
using ProductFocus.Domain;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using ProductFocus.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProductFocusDbContext _context;

        public UnitOfWork(ProductFocusDbContext context)
        {
            _context = context;
        }

        internal T Get<T>(long id)
            where T : class
        {
            return _context.Set<T>().Find(id);
        }

        internal void Insert<T>(T entity) where T: class
        {
            _context.Set<T>().Add(entity);
        }
        internal void Update<T>(T entity) where T : class
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }        
        internal IQueryable<T> Query<T>() where T : class
        {
            return _context.Set<T>();
        }        
        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
