string? present = "alice";
string? missing = null;
Console.WriteLine(present?.Length);
int? length = missing?.Length;
Console.WriteLine(length.HasValue);
Console.WriteLine(present?.ToUpper());
Console.WriteLine(missing?.ToUpper() == null);
