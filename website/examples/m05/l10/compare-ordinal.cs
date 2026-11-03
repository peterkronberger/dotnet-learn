string eventType = "login-failed";

Console.WriteLine(eventType == "LOGIN-FAILED");
Console.WriteLine(string.Equals(eventType, "LOGIN-FAILED", StringComparison.Ordinal));
Console.WriteLine(string.Equals(eventType, "LOGIN-FAILED", StringComparison.OrdinalIgnoreCase));
