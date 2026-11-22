string? name = Environment.GetEnvironmentVariable("SECLOG_USER");

// warning CS8602: Dereference of a possibly null reference.
Console.WriteLine(name.Length);
