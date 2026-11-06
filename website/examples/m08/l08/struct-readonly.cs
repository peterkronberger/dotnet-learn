Distance trip = new(3, 4);
Console.WriteLine(trip.Squared());
Console.WriteLine(trip.Describe());

Speed speed = new(120);
Console.WriteLine(speed.Describe());

readonly struct Speed
{
    public int KmPerHour { get; }

    public Speed(int kmPerHour)
    {
        KmPerHour = kmPerHour;
    }

    public string Describe() => $"{KmPerHour} km/h";
}

struct Distance
{
    public int X { get; set; }
    public int Y { get; set; }

    public Distance(int x, int y)
    {
        X = x;
        Y = y;
    }

    public readonly int Squared() => X * X + Y * Y;

    public readonly string Describe() => $"({X}, {Y})";
}
