string raw = "  Alice ";
string clean = raw.Trim();
string code = raw.Trim().ToUpper().Replace("I", "1");
Console.WriteLine($"[{raw}]");
Console.WriteLine($"[{clean}]");
Console.WriteLine($"[{code}]");
raw = raw.Trim();
Console.WriteLine($"[{raw}]");
