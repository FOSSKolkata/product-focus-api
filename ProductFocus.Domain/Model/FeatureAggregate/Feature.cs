﻿using Common;
using CSharpFunctionalExtensions;
using ProductFocus.Domain.Events;
using ProductFocus.Domain.Model.FeatureAggregate;
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
        public virtual string AcceptanceCriteria { get; set; }
        public virtual string Remarks { get; set; }
        public virtual bool FunctionalTestability { get; set; }

        private readonly IList<UserToFeatureAssignment> _assignees = new List<UserToFeatureAssignment>();
        public virtual IReadOnlyList<UserToFeatureAssignment> Assignees => _assignees.ToList();
        
        private readonly IList<Task> _tasks = new List<Task>();
        public virtual IReadOnlyList<Task> Tasks => _tasks.ToList();

        private readonly IList<FeatureComment> _featureComments = new List<FeatureComment>();
        public virtual IReadOnlyList<FeatureComment> FeatureComments => _featureComments.ToList();

        private readonly IList<ScrumDay> _scrumDays = new List<ScrumDay>();
        public virtual IReadOnlyList<ScrumDay> ScrumDays => _scrumDays.ToList();

        public virtual string Owner { get; private set; }        
        public virtual DateTime? PlannedStartDate { get; private set; }
        public virtual DateTime? PlannedEndDate { get; private set; }
        public virtual DateTime? ActualStartDate { get; private set; }
        public virtual DateTime? ActualEndDate { get; private set; }
        public virtual int WorkCompletionPercentage { get; private set; }
        public virtual int? StoryPoint { get; private set; }
        public virtual bool IsBlocked { get; set; }
        public virtual Status Status { get; private set; }
        public virtual long ModuleId { get; private set; }
        public virtual Module Module { get; private set; }
        public virtual bool IsDeleted { get; set; }
        public virtual Sprint Sprint { get; set; }
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

        private Feature(Module module, string title, WorkItemType workItemType, Sprint sprint)
        {
            Module = module;
            Title = title;
            WorkItemType = workItemType;
            Sprint = sprint;
        }

        public static Feature CreateInstance(Module module, string title, WorkItemType workItemType, Sprint sprint)
        {
            if (String.IsNullOrEmpty(title))
                throw new Exception("Feature Title name can't be null or empty");

            var feature = new Feature(module, title, workItemType, sprint);
            return feature;
        }

        public virtual void UpdateTitle(string title)
        {
            Title = title;
        }

        public virtual void UpdateDescription(string description)
        {
            Description = description;
        }

        public virtual void UpdateRemarks(string remarks)
        {
            Remarks = remarks;
        }
        public virtual void UpdateFunctionalTestability(bool functionalTestability)
        {
            FunctionalTestability = functionalTestability;
        }

        public virtual void UpdateWorkCompletionPercentage(int workCompletionPercentage)
        {
            WorkCompletionPercentage = workCompletionPercentage;
        }

        public virtual void UpdateStatus(Status status)
        {
            Status = status;
        }

        public virtual void UpdateStoryPoint(int storyPoint)
        {
            StoryPoint = storyPoint;
        }
        public virtual void UpdateBlockedStatus(bool isBlocked, long userId)
        {
            IsBlocked = isBlocked;

            if(isBlocked)
                AddDomainEvent(new WorkItemBlockedDomainEvent(this, userId, Module.ProductId));
        }

        public virtual void UpdateSprint(Sprint sprint)
        {
            Sprint = sprint;
        }

        public virtual void UpdateAcceptanceCriteria(string acceptanceCriteria)
        {
            AcceptanceCriteria = acceptanceCriteria;
        }

        public virtual void IncludeAssignee(User user)
        {
            UserToFeatureAssignment newAssignee = UserToFeatureAssignment.CreateInstance(this, user);
            _assignees.Add(newAssignee);
        }

        public virtual void ExcludeAssignee(User user)
        {
            UserToFeatureAssignment assigneeToExclude = Assignees.SingleOrDefault(x => x.User == user);
            _assignees.Remove(assigneeToExclude);
        }

        public virtual void UpdatePlannedStartDate(DateTime plannedStartDate)
        {
            PlannedStartDate = plannedStartDate;
        }
        public virtual void UpdatePlannedEndDate(DateTime plannedEndDate)
        {
            PlannedEndDate = plannedEndDate;
        }
        public virtual void UpdateActualStartDate(DateTime actualStartDate)
        {
            ActualStartDate = actualStartDate;
        }
        public virtual void UpdateActualEndDate(DateTime actualEndDate)
        {
            ActualEndDate = actualEndDate;
        }

        public virtual Result UpsertScrumComment(DateTime scrumDate, string comment)
        {
            if (!(scrumDate >= this.Sprint.StartDate && scrumDate <= this.Sprint.EndDate))
                return Result.Failure("Invalid scrum date");

            var scrumDay =  this.ScrumDays.Where(x => x.ScrumDate == scrumDate).SingleOrDefault();

            if (scrumDay == null)
                this._scrumDays.Add(new ScrumDay(scrumDate, comment, this));
            else
                scrumDay.UpdateComment(comment);

            return Result.Success();
        }


        public Result UpsertWorkCompletionPercentage(DateTime scrumDate, int workCompletionPercentage)
        {
            if (!(scrumDate >= this.Sprint.StartDate && scrumDate <= this.Sprint.EndDate))
                return Result.Failure("Invalid scrum date");

            var scrumDay = this.ScrumDays.Where(x => x.ScrumDate == scrumDate).SingleOrDefault();

            if (scrumDay == null)
                this._scrumDays.Add(new ScrumDay(scrumDate, workCompletionPercentage, this));
            else
                scrumDay.UpdateCompletionPercentage(workCompletionPercentage);

            return Result.Success();
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
