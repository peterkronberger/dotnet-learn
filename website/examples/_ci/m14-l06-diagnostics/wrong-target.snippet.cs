[Author("Dana")]
class Report
{
}

[AttributeUsage(AttributeTargets.Method)]
class AuthorAttribute : Attribute
{
    public AuthorAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}
