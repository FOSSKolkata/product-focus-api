﻿using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Model
{
    public class Feature : AggregateRoot<long>, ISoftDeletable
    {
        public virtual string Title { get; set; }
        public virtual int UniqueWorkItemNumber { get; set; }
        public virtual string Description { get; set; }
        public virtual List<Task> Tasks { get; set; }
        public virtual List<FeatureComment> FeatureComments { get; set; }
        public virtual string Owner { get; set; }
        public virtual DateTime PlannedStartDate { get; set; }
        public virtual DateTime PlannedEndDate { get; set; }
        public virtual DateTime ActualStartDate { get; set; }
        public virtual DateTime ActualEndDate { get; set; }
        public virtual int WorkCompletionPercentage { get; set; }
        public virtual Status Status { get; set; }
        public virtual int ModuleId { get; set; }
        public virtual Module Module { get; set; }
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
        New,
        InProgress,
        OnHold,
        Complete
    }
}
