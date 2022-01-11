using MediatR;
using ProductFocus.Domain.Model;

namespace ProductFocus.Domain.Events
{
    public class WorkItemTitleChangedDomainEvent : INotification
    {
        public Feature Feature { get; set; }
        public long EventTriggeredById { get; set; }
        public long ProductId { get; set; }
        public string PreviousTitle { get; set; }
        public string CurrentTitle { get; set; }
        public WorkItemTitleChangedDomainEvent(Feature feature, long eventTriggeredById, long productId, string previousTitle, string currentTitle)
        {
            Feature = feature;
            EventTriggeredById = eventTriggeredById;
            ProductId = productId;
            PreviousTitle = previousTitle;
            CurrentTitle = currentTitle;
        }
    }
}
