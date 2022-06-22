using ProductFocus.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Model
{
    public class Release : AggregateRoot<long>
    {
        public long ProductId { get; private set; }
        public string Name { get; private set; }
        public DateTime? ReleaseDate { get; private set; }
        protected Release()
        {

        }
        private Release(long productId, string name, DateTime? releaseDate)
        {
            ProductId = productId;
            Name = name;
            ReleaseDate = releaseDate;
        }
        public static Release CreateInstance(long productId, string name, DateTime? releaseDate)
        {
            return new(productId, name, releaseDate);
        }
    }
}
