using System;
using System.Collections.Generic;

namespace ProductFocusApi.Dtos
{
    public sealed class GetDomainEventLogDto
    {
        public Guid Id { get; set; }
        public string EventTypeName { get; set; }
        public string DomainEventJson { get; set; }
        public long ModuleId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public long ProductId { get; set; }
        public string ModuleName { get; set; }
        public List<Owner> Owners { get; set; }
    }
    public sealed class Owner 
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ObjectId { get; set; }
        public Guid EventId { get; }
    }
}
