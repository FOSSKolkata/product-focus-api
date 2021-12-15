namespace ProductFocus.Dtos
{
    public sealed class GetOrganizationByUserDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsOwner { get; set; }
    }
}
