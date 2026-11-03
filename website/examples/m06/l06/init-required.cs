Alert first = new() { Title = "Many failed logins", Level = 3 };
Alert second = new() { Title = "Disk full" };
Console.WriteLine($"{first.Title}: level {first.Level}");
Console.WriteLine($"{second.Title}: level {second.Level}");

class Alert
{
    public required string Title { get; init; }
    public int Level { get; init; } = 1;
}
