namespace ProductFocus.Dtos
{
    public sealed class CancelInvitationDto
    {
        public long InvitationId { get; set; }
        public long OrgId { get; set; }
        public string Email { get; set; }                
    }
}
