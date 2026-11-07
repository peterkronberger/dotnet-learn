string line = "LOGIN-FAILED user=ana";

bool found = line.StartsWith("login-failed", StringComparison.OrdinalIgnoreCase);
bool exact = line.StartsWith("login-failed", StringComparison.Ordinal);

Console.WriteLine(found);
Console.WriteLine(exact);
