List<string> first = new List<string>();
List<string> second = first;
second.Add("alice");
Console.WriteLine(first.Count);
