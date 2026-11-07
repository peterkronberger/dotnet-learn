Incident incident = new("ana");
Console.WriteLine(incident.Describe());
Console.WriteLine(incident.User);

class Incident(string user)
{
    public string User => user;

    public string Describe()
    {
        return $"incident for {user}";
    }
}
