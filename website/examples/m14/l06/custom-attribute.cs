Report report = new();
report.Print();

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
class AuthorAttribute : Attribute
{
    public AuthorAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
    public int Version { get; set; }
}

[Author("Dana", Version = 2)]
class Report
{
    [Author("Dana")]
    public void Print()
    {
        Console.WriteLine("Report printed");
    }
}
