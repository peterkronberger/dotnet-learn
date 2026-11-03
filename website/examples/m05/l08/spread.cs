string[] early = ["alice", "bob"];
string[] late = ["carol"];
List<string> all = [.. early, "dave", .. late];

Console.WriteLine(all.Count);
foreach (string name in all)
{
    Console.WriteLine(name);
}

List<string> copy = [.. all];
copy.Add("eve");
Console.WriteLine(all.Count);
Console.WriteLine(copy.Count);
