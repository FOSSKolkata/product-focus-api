using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Dtos
{
    public sealed class RejectInvitationDto
    {
        public long InvitationId { get; set; }
        public long OrgId { get; set; }
        public string Email { get; set; }                
    }
}
