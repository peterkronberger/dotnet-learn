Job job = new();
job.Run();

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
class TagAttribute : Attribute
{
    public TagAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}

class Job
{
    [Tag("fast")]
    [Tag("safe")]
    public void Run()
    {
        Console.WriteLine("Job ran");
    }
}
