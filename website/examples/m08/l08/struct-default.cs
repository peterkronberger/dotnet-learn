Point origin = default;
Console.WriteLine($"{origin.X} {origin.Y}");

Point[] points = new Point[2];
Console.WriteLine($"{points[1].X} {points[1].Y}");

Counter fresh = new();
Counter empty = default;
Console.WriteLine(fresh.Value);
Console.WriteLine(empty.Value);

struct Point
{
    public int X { get; set; }
    public int Y { get; set; }
}

struct Counter
{
    public int Value { get; }

    public Counter()
    {
        Value = 5;
    }
}
