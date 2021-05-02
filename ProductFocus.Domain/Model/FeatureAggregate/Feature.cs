using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Model
{
    public class Feature : AggregateRoot<long>, ISoftDeletable
    {
        public virtual string Title { get; private set; }
        public virtual WorkItemType WorkItemType { get; private set;}
        public virtual int UniqueWorkItemNumber { get; private set; }
        public virtual string Description { get; private set; }
        private readonly IList<Task> _tasks = new List<Task>();
        public virtual IReadOnlyList<Task> Tasks => _tasks.ToList();
        private readonly IList<FeatureComment> _featureComments = new List<FeatureComment>();
        public virtual IReadOnlyList<FeatureComment> FeatureComments => _featureComments.ToList();
        public virtual string Owner { get; private set; }
        public virtual DateTime PlannedStartDate { get; private set; }
        public virtual DateTime PlannedEndDate { get; private set; }
        public virtual DateTime ActualStartDate { get; private set; }
        public virtual DateTime ActualEndDate { get; private set; }
        public virtual int WorkCompletionPercentage { get; private set; }
        public virtual Status Status { get; private set; }
        public virtual long ModuleId { get; private set; }
        public virtual Module Module { get; private set; }
        public virtual bool IsDeleted { get; set; }
        public virtual DateTime DeletedOn { get; set; }
        public virtual string DeletedBy { get; set; }

        protected Feature()
        {

        }

        private Feature(Module module, string title, string description,
            int progress) : this()
        {
            Module = module;
            Title = title;
            Description = description;
            WorkCompletionPercentage = progress;
        }

        private Feature(Module module, string title, WorkItemType workItemType)
        {
            Module = module;
            Title = title;
            WorkItemType = WorkItemType;
        }

        public static Feature CreateInstance(Module module, string title, WorkItemType workItemType)
        {
            if (String.IsNullOrEmpty(title))
                throw new Exception("Feature Title name can't be null or blank");

            var feature = new Feature(module, title, workItemType);
            return feature;
        }
    }

    public enum Status
    {
        New = 1,
        InProgress = 2,
        OnHold = 3,
        Complete = 4
    }

    public enum WorkItemType
    {
        Feature = 1,
        Bug =2
    }
}
