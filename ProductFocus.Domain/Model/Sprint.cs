using Common;
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
    }
}
