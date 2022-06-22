using Releases.Domain.Common;

namespace Releases.Domain.Model
{
    public class Release : AggregateRoot<long>
    {
        public long ProductId { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public DateTime? ReleaseDate { get; private set; }
        private readonly IList<ReleaseWorkItemCount> _workItemsCount = new List<ReleaseWorkItemCount>();
        public virtual IReadOnlyList<ReleaseWorkItemCount> WorkItemsCount => _workItemsCount.ToList();
        protected Release()
        { }

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
