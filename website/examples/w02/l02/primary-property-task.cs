Incident incident = new("ana");
Console.WriteLine(incident.Describe());
// TODO: add a property User to Incident and print incident.User here

class Incident(string user)
{
    public string Describe()
    {
        return $"incident for {user}";
    }
}
