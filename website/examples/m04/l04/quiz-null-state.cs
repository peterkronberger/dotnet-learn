string? first = null;
string? second = first;
Console.WriteLine(second == null);
first = "bob";
Console.WriteLine(second == null);
Console.WriteLine(first == null);
