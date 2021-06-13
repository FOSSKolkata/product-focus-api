using ProductFocus.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Dtos
{
    public sealed class GetClosedInvitationDto
    {
        public int RecordCount { get; set; }
        public IList<ClosedInvitationDetails> ClosedInvitations { get; set; }
    }

    public sealed class ClosedInvitationDetails
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public long OrganizationId { get; set; }
        public DateTime InvitedOn { get; set; }
        public DateTime ActionedOn { get; set; }
        public InvitationStatus Status { get; set; }
    }
}
