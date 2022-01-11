namespace ProductFocusApi.Dtos
{
    public sealed class GetInvitationDetailsDto
    {
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string OrganizationName { get; set; }
    }
    public sealed class InvitationDetails
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public long OrganizationId { get; set; }
        public long CreatedById { get; set; }
        public long Status { get; set; }
    }
}
