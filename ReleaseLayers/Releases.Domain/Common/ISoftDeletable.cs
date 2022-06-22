using System;

namespace Releases.Domain.Common
{
    public interface ISoftDeletable
    {
        public bool IsDeleted { get; set; }
        public DateTime DeletedOn { get; set; }
        public string DeletedBy { get; set; }
    }
}
