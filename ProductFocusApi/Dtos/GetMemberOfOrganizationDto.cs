using System.Collections.Generic;

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
