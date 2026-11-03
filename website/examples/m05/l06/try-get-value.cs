Dictionary<string, int> failures = new Dictionary<string, int>();
failures["bob"] = 5;

bool found = failures.TryGetValue("bob", out int bobCount);
Console.WriteLine($"{found} {bobCount}");

found = failures.TryGetValue("carol", out int carolCount);
Console.WriteLine($"{found} {carolCount}");

failures.Remove("bob");
Console.WriteLine(failures.Count);
