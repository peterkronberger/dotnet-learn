var roster = new Roster();
roster.Names.Add("ana");
Console.WriteLine(roster.Names.Count);

class Roster
{
    public readonly List<string> Names = new();
}
