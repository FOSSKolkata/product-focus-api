using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Dtos
{
    public sealed class AcceptInvitationDto
    {
        public long OrgId { get; set; }
        public string Email { get; set; }                
    }
}
