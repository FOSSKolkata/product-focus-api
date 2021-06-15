using ProductFocus.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Dtos
{
    public sealed class GetMemberOfOrganizationDto
    {
        public int RecordCount { get; set; }
        public IList<MemberDetails> Members { get; set; }
    }

    public sealed class MemberDetails
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsOwner { get; set; }
    }
}
