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
        protected Sprint()
        {

        }

        public Sprint(string name, DateTime startDate, DateTime endDate)
        {
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
        }

        public static Result<Sprint> CreateInstance(string name, DateTime startDate, DateTime endTime)
        {
            if (String.IsNullOrEmpty(name))
                return Result.Failure<Sprint>("Sprint name can't be null or empty");

            if(startDate == default || endTime == default)
                return Result.Failure<Sprint>("Invalid start date and/or end date");

            var sprint = new Sprint(name, startDate, endTime);
            return sprint;
        }
    }
}
