namespace COSXML.Model.Tag
{
    public sealed class DomainConfiguration
    {
        public DomainRule rule;

        public sealed class DomainRule
        {
            public string Name;
            public string Status;
            public string Type;
            public string Replace;
        }
    }
}
