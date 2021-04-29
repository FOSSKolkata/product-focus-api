using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Model
{
    public class Feature : AggregateRoot<long>, ISoftDeletable
    {
        public virtual string Title { get; protected set; }
        public virtual int UniqueWorkItemNumber { get; protected set; }
        public virtual string Description { get; protected set; }
        private readonly IList<Task> _tasks = new List<Task>();
        public virtual IReadOnlyList<Task> Tasks => _tasks.ToList();
        private readonly IList<FeatureComment> _featureComments = new List<FeatureComment>();
        public virtual IReadOnlyList<FeatureComment> FeatureComments => _featureComments.ToList();
        public virtual string Owner { get; protected set; }
        public virtual DateTime PlannedStartDate { get; protected set; }
        public virtual DateTime PlannedEndDate { get; protected set; }
        public virtual DateTime ActualStartDate { get; protected set; }
        public virtual DateTime ActualEndDate { get; protected set; }
        public virtual int WorkCompletionPercentage { get; protected set; }
        public virtual Status Status { get; protected set; }
        public virtual long ModuleId { get; protected set; }
        public virtual Module Module { get; protected set; }
        public virtual bool IsDeleted { get; set; }
        public virtual DateTime DeletedOn { get; set; }
        public virtual string DeletedBy { get; set; }

        protected Feature()
        {

        }

        public Feature(Module module, string title, string description,
            int progress) : this()
        {
            Module = module;
            Title = title;
            Description = description;
            WorkCompletionPercentage = progress;
        }
    }

    public enum Status
    {
        New = 1,
        InProgress = 2,
        OnHold = 3,
        Complete = 4
    }
}
