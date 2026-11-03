string text = "login-failed user=bob";

Console.WriteLine(text.Contains("failed", StringComparison.Ordinal));
Console.WriteLine(text.StartsWith("logout", StringComparison.Ordinal));
Console.WriteLine(text.IndexOf("user=", StringComparison.Ordinal));
Console.WriteLine(text.IndexOf("carol", StringComparison.Ordinal));
