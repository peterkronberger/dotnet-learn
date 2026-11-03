Alert alert = new Alert("Many failed logins");
alert.Level = 3;
Console.WriteLine($"{alert.Title}: level {alert.Level}");
Console.WriteLine($"Source: {alert.Source}");

class Alert
{
    public string Title { get; }
    public int Level { get; set; }
    public string Source { get; } = "manual";

    public Alert(string title)
    {
        Title = title;
    }
}
