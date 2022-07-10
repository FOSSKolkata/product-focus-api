namespace BusinessRequirements.ConnectionString
{
    public sealed class BlobConnectionString
    {
        public string Value { get; }
        public BlobConnectionString(string value)
        {
            Value = value;
        }
    }
}
