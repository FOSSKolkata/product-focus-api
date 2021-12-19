namespace ProductFocusApi.Dtos
{
    public sealed class AddTagDto
    {
        public string Name { get; set; }
        public long? TagCategoryId { get; set; }
    }
}
