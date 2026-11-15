List<string> monday = ["ana", "ben", "carol"];
List<string> tuesday = ["ben", "dora"];

IEnumerable<string> everyone = monday.Union(tuesday);
IEnumerable<string> allVisits = monday.Concat(tuesday);

Console.WriteLine(everyone.Count());
Console.WriteLine(allVisits.Count());
