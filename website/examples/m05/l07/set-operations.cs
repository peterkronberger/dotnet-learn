HashSet<string> morning = new HashSet<string> { "alice", "bob" };
HashSet<string> evening = new HashSet<string> { "bob", "carol" };

HashSet<string> both = new HashSet<string>(morning);
both.IntersectWith(evening);
Console.WriteLine(both.Count);
Console.WriteLine(both.Contains("bob"));

HashSet<string> either = new HashSet<string>(morning);
either.UnionWith(evening);
Console.WriteLine(either.Count);

HashSet<string> onlyMorning = new HashSet<string>(morning);
onlyMorning.ExceptWith(evening);
Console.WriteLine(onlyMorning.Count);
Console.WriteLine(onlyMorning.Contains("alice"));
