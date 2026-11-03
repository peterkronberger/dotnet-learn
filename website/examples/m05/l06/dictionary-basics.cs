Dictionary<string, int> failures = new Dictionary<string, int>();
failures.Add("bob", 5);
failures["alice"] = 1;
failures["alice"] = 2;
Console.WriteLine(failures.Count);
Console.WriteLine(failures["alice"]);
Console.WriteLine(failures["bob"]);
Console.WriteLine(failures.ContainsKey("carol"));
