Dictionary<string, int> stock = new Dictionary<string, int>();
stock["a"] = 1;
stock["b"] = 2;
stock["a"] = 10;
stock.Remove("b");

stock.TryGetValue("b", out int bValue);
Console.WriteLine(stock.Count);
Console.WriteLine(stock["a"]);
Console.WriteLine(bValue);
