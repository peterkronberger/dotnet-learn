Alert alert = new Alert("Many failed logins", 3);
Console.WriteLine($"{alert.Title} / {alert.Level}");
Console.WriteLine(alert.Describe());

class Alert(string title, int level)
{
    // The parameter title is read by a property that we declare
    public string Title => title;

    // The parameter level only initializes a property
    public int Level { get; } = level;

    public string Describe()
    {
        return $"{Title}: level {Level}";
    }
}
