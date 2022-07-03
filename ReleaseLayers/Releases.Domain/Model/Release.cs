using Releases.Domain.Common;

namespace Releases.Domain.Model
{
    public class Release : AggregateRoot<long>
    {
        public long ProductId { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public DateTime? ReleaseDate { get; private set; }
        public ReleaseStatus Status { get; private set; }
        private readonly IList<ReleaseWorkItemCount> _workItemsCount = new List<ReleaseWorkItemCount>();
        public virtual IReadOnlyList<ReleaseWorkItemCount> WorkItemsCount => _workItemsCount.ToList();
        protected Release()
        { }

        private Release(long productId, string name, DateTime? releaseDate, ReleaseStatus status)
        {
            ProductId = productId;
            Name = name;
            ReleaseDate = releaseDate;
            Status = status;
        }
        public static Release CreateInstance(long productId, string name, DateTime? releaseDate, ReleaseStatus releaseStatus)
        {
            return new(productId, name, releaseDate, releaseStatus);
        }
        public void UpdateName(string name)
        {
            Name = name;
        }
        public void UpdateReleaseDate(DateTime releaseDate)
        {
            ReleaseDate = releaseDate;
        }

        public void UpdateStatus(ReleaseStatus status)
        {
            Status = status;
        }
    }

    public enum ReleaseStatus
    {
        NotStarted = 1,
        Progress = 2,
        Completed = 3,
    }
}
