using ProductFocus.Domain.Common;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductFocus.Domain.Model
{
    public class Sprint : AggregateRoot<long>, ISoftDeletable
    {
        public virtual string Name { get; private set; }
        public virtual DateTime StartDate { get; private set; }
        public virtual DateTime EndDate { get; private set; }
        public long ProductId { get; private set; }
        public virtual Product Product { get; private set; } // virtual keyword is added for lazy loading to work
        public bool IsDeleted { get; set; }
        public DateTime DeletedOn { get; set; }
        public string DeletedBy { get; set; }

        protected Sprint()
        {
            // this protected constructor is for lazy loading to work
        }

        private Sprint(Product  product, string name, DateTime startDate, DateTime endDate)
        {
            Product = product;
            ProductId = product.Id;
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
        }

        public static Result<Sprint> CreateInstance(Product product, string name, DateTime startDate, DateTime endTime)
        {
            if (String.IsNullOrEmpty(name))
                return Result.Failure<Sprint>("Sprint name can't be null or empty");

            if(startDate == default || endTime == default)
                return Result.Failure<Sprint>("Invalid start date and/or end date");

            var sprint = new Sprint(product, name, startDate, endTime);
            return sprint;
        }

        public void Update(string name, DateTime startDate, DateTime endDate)
        {
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
        }
        public void Delete(string userId)
        {
            IsDeleted = true;
            DeletedBy = userId;
            DeletedOn = DateTime.Now;
        }
    }
}
