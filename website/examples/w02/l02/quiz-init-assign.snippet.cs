Incident incident = new() { Source = "vpn", Count = 1 };
incident.Source = "mail";

class Incident
{
    public string Source { get; init; } = "";
    public int Count { get; set; }
}
