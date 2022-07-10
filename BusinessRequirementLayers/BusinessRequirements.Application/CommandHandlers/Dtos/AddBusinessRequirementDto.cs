using BusinessRequirements.Domain.Model;

namespace BusinessRequirements.CommandHandlers.Dtos
{
    public sealed class AddBusinessRequirementDto
    {
        public long? Id { get; set; }
        public long ProductId { get; set; }
        public string Title { get; set; }
        public DateTime ReceivedOn { get; set; }
        public IList<long> TagIds { get; set; }
        public BusinessRequirementSourceEnum SourceEnum { get; set; }
        public string SourceAdditionalInformation { get; set; }
        public string Description { get; set; }
    }
}
