int? a = 4;
int? b = null;
Console.WriteLine(a + 1);
Console.WriteLine(a + b == null);
Console.WriteLine(a > b);
Console.WriteLine(b.GetValueOrDefault() + 2);
