using ProductFocus.Domain.Model;
using System;
using System.Collections.Generic;

namespace ProductFocus.Dtos
{
    public sealed class GetPendingInvitationDto
    {
        public int RecordCount { get; set; }
        public IList<InvitationDetails> PendingInvitations { get; set; }
    }

    public sealed class InvitationDetails
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public long OrganizationId { get; set; }
        public DateTime InvitedOn { get; set; }
        public DateTime LastResentOn { get; set; }
        public InvitationStatus Status { get; set; }
    }
}
