using MediatR;
using ProductFocus.Domain.Model;

namespace ProductFocus.Domain.Events
{
    public class AddWorkItemToSprintDomainEvent : INotification
    {
        public Feature Feature { get; set; }
        public long EventTriggeredById { get; set; }
        public long ProductId { get; set; }
        public long PreviousSprintId { get; set; }
        public long CurrentSprintId { get; set; }
        public string PreviousSprint { get; set; }
        public string CurrentSprint { get; set; }
        public AddWorkItemToSprintDomainEvent(Feature feature, long eventTriggeredById, long productId, Sprint previousSprint, Sprint currentSprint)
        {
            Feature = feature;
            EventTriggeredById = eventTriggeredById;
            ProductId = productId;
            PreviousSprint = previousSprint.Name;
            CurrentSprint = currentSprint.Name;
            PreviousSprintId = previousSprint.Id;
            CurrentSprintId = currentSprint.Id;
        }
    }
}
