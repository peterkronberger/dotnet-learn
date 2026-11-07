Incident incident = new() { Source = "vpn", Count = 1 };
incident.Count = 2;

Console.WriteLine($"{incident.Source}: {incident.Count}");

class Incident
{
    public required string Source { get; init; }
    public int Count { get; set; }
}
