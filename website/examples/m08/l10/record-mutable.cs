Team team = new("ops");
team.Members.Add("alice");
team.Status = "active";
Console.WriteLine($"{team.Name}: {team.Members.Count} member, {team.Status}");

Team same = new("ops");
Console.WriteLine(team == same);

record Team(string Name)
{
    public List<string> Members { get; } = [];
    public string Status { get; set; } = "new";
}
