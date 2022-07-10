using CSharpFunctionalExtensions;
using ProductFocus.Domain.Common;
using ProductFocus.Domain.Events;
using ProductFocus.Domain.Model.FeatureAggregate;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public virtual long? ReleaseId { get; private set; }
        public virtual Release Release { get; private set; }

        public virtual string Owner { get; private set; }        
        public virtual DateTime? PlannedStartDate { get; private set; }
        public virtual DateTime? PlannedEndDate { get; private set; }
        public virtual DateTime? ActualStartDate { get; private set; }
        public virtual DateTime? ActualEndDate { get; private set; }
        public virtual int WorkCompletionPercentage { get; private set; }
        public virtual int? StoryPoint { get; private set; }
        public virtual bool IsBlocked { get; set; }
        public virtual Status Status { get; private set; }
        public virtual long? ModuleId { get; private set; }
        public virtual Module Module { get; private set; }
        public virtual long ProductId { get; private set; }
        public virtual bool IsDeleted { get; set; }
        public virtual Sprint Sprint { get; set; }
        public virtual DateTime DeletedOn { get; set; }
        public virtual string DeletedBy { get; set; }

        protected Feature()
        {

        }

        /*private Feature(string title, string description,
            int progress) : this()
        {
            Title = title;
            Description = description;
            WorkCompletionPercentage = progress;
        }*/

        private Feature(Product product, string title, WorkItemType workItemType, Sprint sprint, long userId)
        {
            ProductId = product.Id;
            Title = title;
            WorkItemType = workItemType;
            Sprint = sprint;
            AddDomainEvent(new AddWorkItemDomainEvent(this, userId, Sprint?.Id, ProductId));
        }

        public static Feature CreateInstance(Product product, string title, WorkItemType workItemType, Sprint sprint, long userId)
        {
            if (String.IsNullOrEmpty(title))
                throw new Exception("Feature Title name can't be null or empty");

            var feature = new Feature(product, title, workItemType, sprint, userId);
            return feature;
        }

        public virtual void UpdateTitle(string title, long userId, string previousTitle)
        {
            Title = title;
            AddDomainEvent(new WorkItemTitleChangedDomainEvent(this, userId, ProductId, previousTitle, title));
        }

        public virtual void UpdateDescription(string description, long userId, string previousDescription)
        {
            Description = description;
            AddDomainEvent(new WorkItemDescriptionChangedDomainEvent(this, userId, ProductId, previousDescription, description));
        }

        public virtual void UpdateRelease(Release release)
        {
            Release = release;
            ReleaseId = release?.Id;
        }

        public virtual void UpdateRemarks(string remarks)
        {
            Remarks = remarks;
        }
        public virtual void UpdateFunctionalTestability(bool functionalTestability)
        {
            FunctionalTestability = functionalTestability;
        }

        public virtual void UpdateWorkCompletionPercentage(int workCompletionPercentage, long userId, long oldWorkPercentage)
        {
            WorkCompletionPercentage = workCompletionPercentage;
            AddDomainEvent(new WorkInProgressDomainEvent(this, userId, ProductId, oldWorkPercentage, workCompletionPercentage));
        }

        public virtual void UpdateStatus(Status status)
        {
            Status = status;
        }

        public virtual void UpdateStoryPoint(int storyPoint, long userId, long? previousStoryPoint)
        {
            StoryPoint = storyPoint;
            AddDomainEvent(new WorkItemStoryPointChangedDomainEvent(this, userId, ProductId, previousStoryPoint, storyPoint));
        }
        public virtual void UpdateBlockedStatus(bool isBlocked, long userId)
        {
            IsBlocked = isBlocked;

            if(isBlocked)
                AddDomainEvent(new WorkItemBlockedDomainEvent(this, userId, ProductId));
        }

        public virtual void UpdateSprint(Sprint currentSprint, long userId, Sprint previousSprint)
        {
            Sprint = currentSprint;
            AddDomainEvent(new AddWorkItemToSprintDomainEvent(this, userId, ProductId, previousSprint, currentSprint));
        }

        public virtual void UpdateAcceptanceCriteria(string acceptanceCriteria)
        {
            AcceptanceCriteria = acceptanceCriteria;
        }

        public virtual void IncludeAssignee(User user, long userId, string email)
        {
            UserToFeatureAssignment newAssignee = UserToFeatureAssignment.CreateInstance(this, user);
            _assignees.Add(newAssignee);
            AddDomainEvent(new AddOwnerToWorkItemDomainEvent(this, userId, ProductId, user.Name, email));
        }

        public virtual void ExcludeAssignee(User user, long userId, string email)
        {
            UserToFeatureAssignment assigneeToExclude = Assignees.SingleOrDefault(x => x.User == user);
            _assignees.Remove(assigneeToExclude);
            AddDomainEvent(new RemoveOwnerFromWorkItemDomainEvent(this, userId, ProductId, user.Name, email));
        }

        public virtual void UpdatePlannedStartDate(DateTime plannedStartDate, long userId, DateTime? previousEndDate)
        {
            PlannedStartDate = plannedStartDate;
            AddDomainEvent(new WorkItemStartDateChangedDomainEvent(this, userId, ProductId, previousEndDate, plannedStartDate));
        }
        public virtual void UpdatePlannedEndDate(DateTime plannedEndDate, long userId, DateTime? previousEndDate)
        {
            PlannedEndDate = plannedEndDate;
            AddDomainEvent(new WorkItemEndDateChangedDomainEvent(this, userId, ProductId, previousEndDate, plannedEndDate));
        }
        public virtual void UpdateActualStartDate(DateTime actualStartDate)
        {
            ActualStartDate = actualStartDate;
        }
        public virtual void UpdateActualEndDate(DateTime actualEndDate)
        {
            ActualEndDate = actualEndDate;
        }

        public virtual void UpdateModule(Module module)
        {
            Module = module;
            ModuleId = module?.Id;
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
        Planned = 2,
        DevInProgress = 3,
        SFQ = 4,
        Done = 5,
    }

    public enum WorkItemType
    {
        Feature = 1,
        Bug = 2,
        Epic = 3,
        Pbi = 4
    }
}
