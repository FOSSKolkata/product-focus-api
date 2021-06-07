using ProductFocus.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Dtos
{
    public sealed class GetPendingInvitationDto
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public long OrganizationId { get; set; }
        public DateTime InvitedOn { get; set; }
        public DateTime LastResentOn { get; set; }
        public InvitationStatus Status { get; set; }
    }
}
