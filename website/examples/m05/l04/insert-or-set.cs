List<string> inserted = new List<string>();
inserted.Add("alice");
inserted.Add("bob");
inserted.Add("dave");
inserted.Insert(1, "carol");
Console.WriteLine($"Insert: {inserted.Count} {inserted[1]} {inserted[2]}");

List<string> replaced = new List<string>();
replaced.Add("alice");
replaced.Add("bob");
replaced.Add("dave");
replaced[1] = "carol";
Console.WriteLine($"Set: {replaced.Count} {replaced[1]} {replaced[2]}");
