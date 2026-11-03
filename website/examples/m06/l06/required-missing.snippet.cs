Alert alert = new() { Level = 3 };

class Alert
{
    public required string Title { get; init; }
    public int Level { get; init; } = 1;
}
