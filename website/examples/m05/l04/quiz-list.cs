List<int> counts = new List<int>();
counts.Add(5);
counts.Add(1);
counts.Insert(0, 3);
counts.RemoveAt(1);
Console.WriteLine(counts.Count);
Console.WriteLine(counts[0]);
Console.WriteLine(counts[1]);
