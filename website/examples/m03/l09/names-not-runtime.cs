(string User, int Failures) first = ("bob", 5);
(string Name, int Count) second = first;
Console.WriteLine($"{second.Name} {second.Count}");
Console.WriteLine(second.Item1);
