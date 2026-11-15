Report report = new();
Console.WriteLine(report.Title());

class Report
{
    public string Title()
    {
        return "instance method";
    }
}

static class ReportExtensions
{
    public static string Title(this Report report)
    {
        return "extension method";
    }
}
