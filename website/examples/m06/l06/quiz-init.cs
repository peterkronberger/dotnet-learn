Alert alert = new() { Title = "Disk full", Level = 5 };
Console.WriteLine($"{alert.Title}: {alert.Level}");

class Alert
{
    public required string Title { get; init; }

    public int Level
    {
        get => field;
        init => field = value > 3 ? 3 : value;
    }
}
