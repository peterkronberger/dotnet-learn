string? ip = null;
ip ??= "unknown";
Console.WriteLine(ip);
ip ??= "192.0.2.10";
Console.WriteLine(ip);
