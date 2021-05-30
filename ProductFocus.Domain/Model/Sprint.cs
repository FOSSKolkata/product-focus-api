using Common;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductFocus.Domain.Model
{
    public class Sprint : AggregateRoot<long>
    {
        public virtual string Name { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        public long ProductId { get; private set; }
        public virtual Product Product { get; private set; } // virtual keyword is added for lazy loading to work
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
    }
}
